using project_chess_DB.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System;
using System.Linq;
using Tmds.DBus.Protocol;
namespace project_chess_DB.ViewModels;

public partial class TournamentPageViewModel : ViewModelBase
{
    public ObservableCollection<Tournament> Tournaments { get; set; }

    public string NewName_of_the_tournament { get; set; } = string.Empty;
    public string NewCountry { get; set; } = string.Empty;
    public string NewCity { get; set; } = string.Empty;
    public string NewStart_date { get; set; } = string.Empty;
    public string NewEnd_date { get; set; } = string.Empty;
    public ICommand AddTournamentCommand { get; }
    public ICommand DeleteTournamentCommand { get; }
    public TournamentPageViewModel()
    {
        var tournaments = new List<Tournament>
        {
        };
        Tournaments = new ObservableCollection<Tournament>(tournaments);
        AddTournamentCommand = new RelayCommand1(AddTournament);
        DeleteTournamentCommand = new RelayCommand1(DeleteTournament);
    }
    private void AddTournament()
    {
        var newTournament = new Tournament(
            NewName_of_the_tournament,
            NewCountry,
            NewCity,
            NewStart_date,
            NewEnd_date
        );
        Tournaments.Add(newTournament);
    }
    private void DeleteTournament(object? parameter)
    {
        if (parameter is Tournament tournamentToDelete)
        {
            Tournaments.Remove(tournamentToDelete);
        }
    }
}
public class RelayCommand1 : ICommand
{
    private readonly Action<object?>? _executeWithParam;
    private readonly Action? _executeNoParam;

    public RelayCommand1(Action<object?> execute)   //avec paramètre
    {
        _executeWithParam = execute;
        _executeNoParam = null;
    }
    public RelayCommand1(Action execute)        //code quand on veut seullement ajouter
    {
        _executeNoParam = execute;
        _executeWithParam = null;
    }

    // Modification ici : On définit des accesseurs vides pour faire taire l'avertissement
    public event EventHandler? CanExecuteChanged
    {
        add { }
        remove { }
    }

    public bool CanExecute(object? parameter) => true;
    public void Execute(object? parameter)
    {
        if (_executeWithParam != null)
        {
            _executeWithParam(parameter);
        }
        else
        {
            _executeNoParam?.Invoke();
        }
    }
}