using project_chess_DB.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
namespace project_chess_DB.ViewModels;

public partial class PlayerPageViewModel : ViewModelBase
{
    public ObservableCollection<Player> Players { get; set; }
    public PlayerPageViewModel()
    {
        var players = new List<Player>
            {
                new Player("Saadi", "Kenza","Belgique","23268@ecam.be","20","+32123456987"),
                new Player("Umme", "Kulsum","Belgique","22156@ecam.be","21","+320123456879"),
            };
        Players = new ObservableCollection<Player>(players);
    }
}