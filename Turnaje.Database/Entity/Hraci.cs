namespace Turnaje.Database.Entity
{
    public class Hraci
    {
        public Turnaj Turnaj { get; set; } // Turnaj
        public Uzivatel Uzivatel { get; set; } // Hráč v turnaji
        public int Checkin { get; set; } // Je potvrzený?
        public Uzivatel Majitel { get; set; } //Administrátor turnaje
    }
}
