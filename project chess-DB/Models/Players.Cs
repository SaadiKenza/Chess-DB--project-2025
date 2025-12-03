using System.Runtime.InteropServices;
namespace project_chess_DB.Models
{
    public class Player
    {
        //informations obligatoires
        public string Mail { get; set; }
        public string Country { get; set; }
        public string Age { get; set; }
        public string Phone_number { get; set; }
        public string First_name { get; set; }
        public string Last_name { get; set; }
        public string Matricule { get; set; }
        public string Elo { get; set; }
        public Player(string matricule, string last_name, string first_name, string age, string elo, string country, string mail,  string phone_number )
        {
            
            this.Matricule = matricule;
            this.Last_name = last_name;
            this.First_name = first_name;
            this.Age = age;
            this.Elo = elo;
            this.Country = country;
            this.Mail = mail;               //en majuscule, propriété publique et minuscule propriété temporaire
            this.Phone_number = phone_number;
        
        }

    }
}