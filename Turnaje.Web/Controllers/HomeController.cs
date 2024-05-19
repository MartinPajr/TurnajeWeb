using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Turnaje.Database.Entity;
using Turnaje.Database.Repository;
using Turnaje.Web.Models;

namespace Turnaje.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUzivatelRepository _uzivatelRepository;
        private readonly IUTurnajRepository _turnajRepository;
        private readonly IUZapasRepository _uZapasRepository;
        private readonly IUHraRepository _hraRepository;
        private readonly IUHraciRepository _hraciRepository;

        private List<Turnaj> _turnajList;
        public HomeController(ILogger<HomeController> logger, IUzivatelRepository uzivatelRepository, IUTurnajRepository turnajRepository, IUZapasRepository zapasRepository, IUHraRepository hraRepository, IUHraciRepository hraciRepository)
        {
            _logger = logger;
            _uzivatelRepository = uzivatelRepository;
            _turnajRepository = turnajRepository;
            _turnajList = new List<Turnaj>();
            _uZapasRepository = zapasRepository;
            _hraRepository = hraRepository;
            _hraciRepository = hraciRepository;
        }

        public async Task<IActionResult> Index()
        {
            _turnajList = (List<Turnaj>)await _turnajRepository.GetUpcomingTournamentsAsync();
            return View(_turnajList);
        }
        [HttpGet]
        public async Task<IActionResult> MyTournaments(int? id)
        {
            if(id != null)
            {
                _turnajList = (List<Turnaj>)await _turnajRepository.GetUpcomingTournamentsByOwnerAsync(id);
            }
            
            return View(_turnajList);
        }
        public async Task<IActionResult> NewTournament()
        {
            Turnaj model = new Turnaj();
            model.AllGames = await _hraRepository.GetAllAsync();
            return View(model);
        }

        // POST: Handles form submission
        [HttpPost]
        public async Task<IActionResult> NewTournament(Turnaj model)
        {

            int newId = await _turnajRepository.AddAsync(model);
            // Redirect to a new page or show a success message
            return RedirectToAction("Index", new { id = newId });

            // If the model is invalid, show the form again with validation messages
            return View(model);
        }

        public async Task<IActionResult> Tournament(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound(); // or redirect to another page
            }

            Turnaj tournament = await _turnajRepository.GetByIdAsync(id.Value);
            tournament.Hraci = await _uzivatelRepository.GetByTournamentId(tournament.Id);
            tournament.Zapasy = await _uZapasRepository.GetByTournamentId(tournament.Id);
            foreach (Zapas zapas in tournament.Zapasy)
            {
                if (zapas.Player1 == 0)
                {
                    zapas.Winner = 2;
                }
                if (zapas.Player2 == 0)
                {
                    zapas.Winner = 1;
                }

                zapas.Uzivatel1 = await _uzivatelRepository.GetByIdAsync(zapas.Player1);
                zapas.Uzivatel2 = await _uzivatelRepository.GetByIdAsync(zapas.Player2);
            }

            if (tournament == null)
            {
                return NotFound(); // or handle differently if the tournament is not found
            }

            return View(tournament);
        }
        [HttpGet]
        public async Task<IActionResult> Zapas(int id)
        {
            Zapas zapas = await _uZapasRepository.GetByIdAsync(id);

            if (zapas == null)
            {
                return NotFound();
            }

            zapas.Uzivatel1 = await _uzivatelRepository.GetByIdAsync(zapas.Player1);
            zapas.Uzivatel2 = await _uzivatelRepository.GetByIdAsync(zapas.Player2);

            if (zapas.Player2 != 0)
            {
                zapas.Uzivatel2 = await _uzivatelRepository.GetByIdAsync(zapas.Player2);
            }

            return View(zapas);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> TournamentSettings(int id)
        {
            var tournament = await _turnajRepository.GetByIdAsync(id);
            if (tournament == null)
            {
                return NotFound();
            }

            return View(tournament);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TournamentSettings(Turnaj tournament)
        {
            Turnaj tt = await _turnajRepository.GetByIdAsync(tournament.Id);
            tt.Name = tournament.Name;
            tt.Description = tournament.Description;
            tt.CheckStart = tournament.CheckStart;
            tt.Start = tournament.Start;
            await _turnajRepository.UpdateAsync(tt);
            return View(tournament);
        }
        /*
        [HttpGet]
        public async Task<IActionResult> TournamentRegistration(int id)
        {
            Hraci hraci = new Hraci();
            hraci.Turnaj = await _turnajRepository.GetByIdAsync(id);

            return View(hraci);
        }
        */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TournamentRegistration(Turnaj regis)
        {
            Hraci reg = new Hraci
            {
                TurnajId = regis.Id,
                UzivatelId = regis.RegUser
            };
            if(regis.RegUser != 0)
            {
                if (await _hraciRepository.GetByIdAsync(regis.Id, regis.RegUser) == null)
                {
                    await _hraciRepository.AddAsync(reg);
                    return RedirectToAction("Tournament", new { id = regis.Id });
                }
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StartTournament(int id)
        {

            // Logic to start the tournament and generate bracket
            // loop{pocet hracu / 2 += pocet kol, dokud neni vysledek 1. 
            // 2,4,8,16,32,64,128,256,512,1024
            Turnaj turnaj = await _turnajRepository.GetByIdAsync(id);

            if (turnaj.Style == 1) // Single Elim
            {
                // Fáze 0: Zjištění důležitých parametrů
                int TurnajId = id;
                List<Uzivatel> hraciTurnaje = await _uzivatelRepository.GetByTournamentId(id); // Z Databáze bereme seznam uživatelů
                int pocetHracu = hraciTurnaje.Count();
                int velikostBracketu = 1;
                while (velikostBracketu < pocetHracu)
                {
                    velikostBracketu *= 2; //výpočet velikosti potřebného bracketu
                }
                int pocetKol = (int)Math.Log(velikostBracketu, 2);//zjištění počtu kol podle Logaritmu
                turnaj.PocetKol = pocetKol;

                Random rng = new Random();
                int[] hraciId = hraciTurnaje.Select(h => h.Id).ToArray();
                hraciId = hraciId.OrderBy(a => rng.Next()).ToArray(); // Zamíchání hráčů

                turnaj.Zapasy = new List<Zapas>(); //vytvoření seznamu zápasů turnaje
                int indexHrace = 0; // Index pro sledování, kolik hráčů bylo již přiřazeno

                // Fáze 1: Přiřazení hráčů do Player1.
                for (int i = 0; i < velikostBracketu / 2; i++)
                {
                    Zapas novyZapas = new Zapas
                    {
                        RoundNumber = 1,
                        Turnaj = TurnajId
                    };

                    if (indexHrace < hraciId.Length)
                    {
                        novyZapas.Player1 = hraciId[indexHrace];
                        indexHrace++;
                    }

                    turnaj.Zapasy.Add(novyZapas);
                }

                // Fáze 2: Přiřazení zbývajících hráčů do Player2.
                foreach (Zapas zapas in turnaj.Zapasy)
                {
                    if (indexHrace < hraciId.Length)
                    {
                        zapas.Player2 = hraciId[indexHrace];
                        indexHrace++;
                    }
                    else
                    {
                        break; // Přerušení cyklu, pokud už nejsou další hráči k přiřazení
                    }
                }
                foreach (Zapas zapas in turnaj.Zapasy)
                {
                    if(zapas.RoundNumber == 1 && zapas.Player2 == 0)
                    {
                        zapas.Winner = zapas.Player1;
                    }
                    await _uZapasRepository.AddAsync(zapas);
                }
                // Fáze 3: Generování dalších kol. 
                List<Zapas> zapasyPrvniKolo = await _uZapasRepository.GetByTournamentAndRoundNumberId(TurnajId, 1);
                List<Zapas> zapasyDalsichKol = new List<Zapas>();
                int x = turnaj.Zapasy.Count;
                int roundNum = 2;
                while (x > 1)
                {
                    for (int i = 0; i < x; i = i + 2)
                    {
                        Zapas zapas = new Zapas();
                        zapas.Turnaj = TurnajId;
                        zapas.Predzapas1 = zapasyPrvniKolo[i].Id;
                        zapas.Predzapas2 = zapasyPrvniKolo[i + 1].Id;
                        zapas.RoundNumber = roundNum;
                        zapasyDalsichKol.Add(zapas);
                    }
                    foreach (Zapas zapas in zapasyDalsichKol)
                    {
                        await _uZapasRepository.AddAsync(zapas);
                    }
                    zapasyPrvniKolo = await _uZapasRepository.GetByTournamentAndRoundNumberId(TurnajId, roundNum);
                    zapasyDalsichKol.Clear();
                    x = zapasyPrvniKolo.Count;
                    roundNum++;
                }
                // Fáze 4: Automatické určení vítězů, na které nezbyl oponent v prvním kole.
                List<Zapas> vyherniZapas = new List<Zapas>();
                vyherniZapas = await _uZapasRepository.GetByTournamentAndRoundNumberId(TurnajId, 1);
                foreach (Zapas z in vyherniZapas)
                {
                    if(z.Player2 == 0)
                    {
                        z.Reporter = 0;
                        z.Winner = z.Player1;
                        Zapas pok = await _uZapasRepository.GetByRoundBeforeMatch(z.Id);
                        if (pok.Predzapas1 == z.Id)
                        {
                            pok.Player1 = z.Winner;
                        }
                        else
                        {
                            pok.Player2 = z.Winner;
                        }
                        await _uZapasRepository.UpdateAsync(z);
                        await _uZapasRepository.UpdateAsync(pok);
                    }
                }
            }


            return RedirectToAction("Tournament", new { id = id });
        }
        [HttpPost]
        public async Task<IActionResult> VysledekReport(Zapas zapas)
        {
            Zapas dbsZapas = await _uZapasRepository.GetByIdAsync(zapas.Id);
            if (zapas.Reporter != zapas.Winner && dbsZapas.Winner == 0) //Uživatel nahlašuje prohru -> potvrzujeme výhru oponenta
            {
                dbsZapas.Winner = zapas.Winner;
                await _uZapasRepository.UpdateAsync(dbsZapas);
                Zapas dalsiZapas = await _uZapasRepository.GetByRoundBeforeMatch(zapas.Id);
                if(dalsiZapas.Predzapas1 == zapas.Id)
                {
                    dalsiZapas.Player1 = dbsZapas.Winner;
                }
                else
                {
                    dalsiZapas.Player2 = dbsZapas.Winner;
                }
                await _uZapasRepository.UpdateAsync(dalsiZapas);
            }
            return RedirectToAction("Index");
        }



        [HttpGet]
        public IActionResult NewGame()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> NewGame(Hra game)
        {
            Hra test = await _hraRepository.GetByName(game.Name);
            if (test == null)
            {
                _hraRepository.AddAsync(game);
            }
            return RedirectToAction("Index");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}