namespace Turnaje.Database.Entity
{
    public class Hra
    {
        public int Id { get; set; } // Identifikátor
        public string Name { get; set; } // Název hry

        public string GameId { get; set; } //Herní identifikátor jednotlivého uživatele

    }
}
