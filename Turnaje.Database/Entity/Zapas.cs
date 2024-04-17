namespace Turnaje.Database.Entity
{
    public class Zapas
    {
        public int Id { get; set; } // Identifikátor zápasu
        public int Turnaj { get; set; } // Identifikátor turnaje
        public int Player1 { get; set; } // Identifikátor hráče 1
        public int Player2 { get; set; } // Identifikátor hráče 2
        public int Predzapas1 { get; set; } // Identifikátor předcházejícího zápasu 1
        public int Predzapas2 { get; set; } // Identifikátor předcházejícího zápasu 2
        public Uzivatel Uzivatel1 { get; set; } // Model uživatele 1
        public Uzivatel Uzivatel2 { get; set; } // Model uživatele 2
        public string H1Score { get; set; } // Nahlašovaný výsledek hráče 1
        public string H2Score { get; set; } // Nahlašovaný výsledek hráče 2
        public int RoundNumber { get; set; } // Číslo kola ve kterém se hraje 
        public int Winner { get; set; } // Identifikátor uživatele který vyhrál 
        public int Reporter { get; set; } // Identifikátor uživatele který nahlásil výsledek
    }
}
