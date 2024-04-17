using System.Data;
using System.Data.SqlClient;
using Turnaje.Database.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure cookie authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Your login path
        options.LogoutPath = "/Account/Logout";
        options.ExpireTimeSpan = TimeSpan.FromDays(14); // Set the session timeout
    });

builder.Services.AddScoped<IDbConnection>(provider => new SqlConnection("Server=MAINPC\\MSSQLSERVER01;Database=Turnaje;Trusted_Connection=True;"));
builder.Services.AddScoped<IUzivatelRepository, UzivatelRepository>();
builder.Services.AddScoped<IUTurnajRepository, TurnajRepository>();
builder.Services.AddScoped<IUZapasRepository, ZapasRepository>();
builder.Services.AddScoped<IUHraRepository, HraRepository>();
builder.Services.AddScoped<IUHraciRepository, HraciRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Add authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();