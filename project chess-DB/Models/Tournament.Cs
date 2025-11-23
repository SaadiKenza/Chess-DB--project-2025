using System.Runtime.InteropServices;
namespace project_chess_DB.Models
{
    public class Tournament
    {
        public string Name_of_the_tournament { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Start_date { get; set; }
        public string End_date { get; set; }
        public Tournament(string name_of_the_tournament, string country, string city, string start_date, string end_date)
        {
            this.Name_of_the_tournament = name_of_the_tournament;
            this.Country = country;
            this.City = city;
            this.Start_date = start_date;
            this.End_date = end_date;
        }
    }
}