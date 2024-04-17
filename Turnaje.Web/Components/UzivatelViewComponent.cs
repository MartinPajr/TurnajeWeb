using Microsoft.AspNetCore.Mvc;
using Turnaje.Database.Entity; // Nahraďte YourNamespace skutečným namespacem
using Turnaje.Database.Repository;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

public class UzivatelViewComponent : ViewComponent
{
    private readonly IUzivatelRepository _uzivatelRepository;
    public UzivatelViewComponent(IUzivatelRepository uzivatelRepository)
    {
        _uzivatelRepository = uzivatelRepository;
    }
    public async Task<IViewComponentResult> InvokeAsync()
    {
        Uzivatel model;
        string ser = User.Identity.Name;/*
        if(ser != null)
        {
            model = _uzivatelRepository.GetByName(ser);
        }
        Uzivatel model = _uzivatelRepository.GetByName(ser);// Získání modelu Uzivatel (např. z databáze)
        return View(model);*/
        return null;
    }
}