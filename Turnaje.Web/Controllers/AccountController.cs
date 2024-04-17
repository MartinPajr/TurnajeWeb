using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Turnaje.Database.Entity;
using Turnaje.Database.Repository;
using Turnaje.Web.Controllers;

public class AccountController : Controller
{
    // Constructor and dependencies (if any) here
    private readonly ILogger<HomeController> _logger;
    private readonly IUzivatelRepository _uzivatelRepository;
    private readonly IUHraRepository _hraRepository;

    public AccountController(ILogger<HomeController> logger, IUzivatelRepository uzivatelRepository, IUHraRepository hraRepository)
    {
        _logger = logger;
        _uzivatelRepository = uzivatelRepository;
        _hraRepository = hraRepository;
    }

    // GET: Display Login Page
    [HttpGet]
    public IActionResult Login(string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    // POST: Handle Login Data
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(Uzivatel model, string returnUrl = null)
    {
        /*
        if (!ModelState.IsValid)
        {
            return View(model);
        }*/
        Uzivatel uz = await _uzivatelRepository.GetByName(model.Nickname);
        if (uz == null)
        {
            return RedirectToAction("Index");
        }
        if (BCrypt.Net.BCrypt.Verify(model.Password, uz.Password))
        {
            // Create a claim based on the user's information
            var claims = new List<Claim>
            {
                new Claim("NAME", uz.Nickname),
                new Claim("ID", uz.Id.ToString())
            };
            var claimsIdentity = new ClaimsIdentity(claims, "Login");

            // Sign in the user
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));


            // Redirect to the return URL or a default page
            return RedirectToLocal(returnUrl);

        }
        if (uz.Password != model.Password)
        {
            return RedirectToLocal(returnUrl);
        }
        ;
        return RedirectToLocal("Index");
    }

    // POST: Logout
    [HttpGet]
    //[ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    // GET: Display Password Change Page
    [HttpGet]
    public IActionResult PasswordChange()
    {
        return View();
    }

    // POST: Handle Password Change
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PasswordChange(Uzivatel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Password change logic here

        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    // GET: Display Registration Page
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    // POST: Handle Registration Data
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(Uzivatel model)
    {
        if (!ModelState.IsValid)
        {
            model.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);
            await _uzivatelRepository.AddAsync(model);
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    private IActionResult RedirectToLocal(string returnUrl)
    {
        if (Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }
        else
        {
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
    public async Task<IActionResult> UserProfile(int id)
    {
        int Iid = id;
        Uzivatel uz = await _uzivatelRepository.GetByIdAsync(Iid);
        return View(uz);
    }
    [HttpGet]
    public async Task<IActionResult> UserSettings(int id)
    {
        int Iid = id;
        Uzivatel uz = await _uzivatelRepository.GetByIdAsync(Iid);
        List<Hra> hry = await _hraRepository.GetAllAsync();
        uz.Games = hry;
        uz.MyGames = await _hraRepository.GetHryByUzivatelId(Iid);
        return View(uz);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UserSettings(Uzivatel model)
    {
        if(model.Password != null)
        {
            model.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);
        }
        else
        {
            Uzivatel test = await _uzivatelRepository.GetByIdAsync(model.Id);
            model.Password = test.Password;
        }
        await _uzivatelRepository.UpdateAsync(model);
        return RedirectToAction(nameof(HomeController.Index), "Home");
    }
}