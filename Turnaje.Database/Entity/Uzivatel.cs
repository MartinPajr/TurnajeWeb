namespace Turnaje.Database.Entity
{
    public class Uzivatel
    {
        public int Id { get; set; } // Identifikátor
        public string Nickname { get; set; } // Přezdívka
        public string Password { get; set; } // Heslo
        public string Email { get; set; } // Email
        public string Crypto { get; set; } // Speciální adresa pro možnosti budoucího rozšíření
        public List<Hra> Games { get; set; } // Všechny hry, které může registrovat
        public List<Hra> MyGames { get; set; } // Hry, které uživatel zaregistroval
    }
}
