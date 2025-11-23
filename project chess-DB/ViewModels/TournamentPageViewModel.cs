using project_chess_DB.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
namespace project_chess_DB.ViewModels;

public partial class TournamentPageViewModel : ViewModelBase
{
    public ObservableCollection<Tournament> Tournaments { get; set; }
    public TournamentPageViewModel()
    {
        var tournaments = new List<Tournament>
            {
                new Tournament("Test", "Belgique","Bruxelles","23-11-2025","26-11-2025"),
            };
        Tournaments = new ObservableCollection<Tournament>(tournaments);
    }
}