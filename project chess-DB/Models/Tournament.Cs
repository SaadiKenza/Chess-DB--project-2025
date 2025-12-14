using System.Runtime.InteropServices;
using System.Collections.ObjectModel;
namespace project_chess_DB.Models
{
    public class Tournament
    {
        public string Name_of_the_tournament { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Start_date { get; set; } = string.Empty;
        public string End_date { get; set; } = string.Empty;
        public ObservableCollection<Competition> Competitions { get; set; } = new ObservableCollection<Competition>();
        public ObservableCollection<string> RegisteredPlayers { get; set; } = new ObservableCollection<string>();
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