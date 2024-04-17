namespace Turnaje.Database.Entity
{
    public class Turnaj
    {
        public int Id { get; set; } //Identifikátor
        public string Name { get; set; } //Název turnaje
        public int MaxPlayers { get; set; } //Maximální počet hráčů
        public int Style { get; set; } //1 SE, 2 DE, 3 Groups, 4  Swiss, 5 Ladder
        public List<Hra> AllGames { get; set; } //Všechny hry ve kterých se turnaj může pořádat
        public string Game { get; set; } //Hra ve které se turnaj hraje
        public string Description { get; set; } //Popis turnaje
        public int Seed { get; set; } //Kontrola zda je turnaj již spuštěn
        public List<Uzivatel> Hraci { get; set; } //Hráči turnaje
        public DateTime Start { get; set; } // Datum startu turnaje
        public DateTime CheckStart { get; set; } // Datum kdy se hráči potvrzují do turnaje
        public int PocetHracu { get; set; } // Reálný počet hráčů
        public int PocetZapasu { get; set; } // Počet zápasů
        public int PocetKol { get; set; } // Počet kol turnaje
        public List <Zapas> Zapasy { get; set; } // Zápasy
        public int Majitel { get; set; } // Administrátor turnaje
        public string Pravidla { get; set; } // Pravidla turnaje
    }
}
