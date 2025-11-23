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
        public Player(string last_name, string first_name, string country, string mail, string age, string phone_number, string matricule)
        {
            this.Mail = mail;               //en majuscule, propriété publique et minuscule propriété temporaire
            this.Country = country;
            this.Age = age;
            this.Phone_number = phone_number;
            this.First_name = first_name;
            this.Last_name = last_name;
            this.Matricule = matricule;
        }

    }
}