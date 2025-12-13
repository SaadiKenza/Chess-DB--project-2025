using project_chess_DB.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System;
using System.Linq;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia;
using ReactiveUI;
using Avalonia.Threading;
using System.Threading.Tasks;
using project_chess_DB.Services;
using project_chess_DB.Views;

namespace project_chess_DB.ViewModels;

public partial class TournamentPageViewModel : ViewModelBase
{
    public ObservableCollection<Tournament> Tournaments { get; set; }

    private Tournament? _selectedTournament;
    public Tournament? SelectedTournament
    {
        get => _selectedTournament;
        set
        {
            if (_selectedTournament != value)
            {
                _selectedTournament = value;
                OnPropertyChanged(nameof(SelectedTournament));
                OnPropertyChanged(nameof(Competitions)); 
            }
        }
    }

    public ObservableCollection<Competition> Competitions => 
        SelectedTournament?.Competitions ?? new ObservableCollection<Competition>();

    public string NewName_of_the_tournament { get; set; } = string.Empty;
    public string NewCountry { get; set; } = string.Empty;
    public string NewCity { get; set; } = string.Empty;
    public string NewStart_date { get; set; } = string.Empty;
    public string NewEnd_date { get; set; } = string.Empty;


    public string NewCompetitionDate { get; set; } = string.Empty;
    public string NewCompetitionNumber { get; set; } = string.Empty; 

    public string NewP1_RegNumber { get; set; } = string.Empty;
    public string NewP1_Result { get; set; } = string.Empty;
    public string NewP1_Moves { get; set; } = string.Empty;

    public string NewP2_RegNumber { get; set; } = string.Empty;
    public string NewP2_Result { get; set; } = string.Empty;
    public string NewP2_Moves { get; set; } = string.Empty;

    public ICommand AddTournamentCommand { get; }
    public ICommand DeleteTournamentCommand { get; }
    public ICommand OpenJoinTournamentCommand { get; }
    public ICommand AddCompetitionCommand { get; }

    private TournamentRepository repository;

    public TournamentPageViewModel()
    {
        repository = new TournamentRepository();
    
        var tournamentFromDb = repository.GetAllTournaments();
        Tournaments = new ObservableCollection<Tournament>(tournamentFromDb);
        
        OpenJoinTournamentCommand = ReactiveCommand.CreateFromTask<Tournament>(OpenRegistrationWindow, outputScheduler: RxApp.MainThreadScheduler);
        AddTournamentCommand = new RelayCommand2(AddTournament);
        DeleteTournamentCommand = new RelayCommand2(DeleteTournament);
        AddCompetitionCommand = new RelayCommand2(AddCompetition); 
    }

    // --- METHODES ---
    private async Task OpenRegistrationWindow(Tournament tournament)
    {
        if (tournament == null) return;

        await Dispatcher.UIThread.InvokeAsync(async () =>
        {
            var dialog = new RegisterPlayerPageView();

            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var mainWindow = desktop.MainWindow;
                if (mainWindow is not null)
                {
                    var result = await dialog.ShowDialog<string>(mainWindow);
                    if (!string.IsNullOrEmpty(result))
                    {
                        System.Diagnostics.Debug.WriteLine($"Résultat : {result}");
                    }
                }
            }
        });
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

        repository.AddTournament(newTournament);
        Tournaments.Add(newTournament);
    }

    private void DeleteTournament(object? parameter)
    {
        if (parameter is Tournament tournamentToDelete)
        {
            repository.DeleteTournament(tournamentToDelete.Name_of_the_tournament);
            Tournaments.Remove(tournamentToDelete);
            
            if (SelectedTournament == tournamentToDelete)
            {
                SelectedTournament = null;
            }
        }
    }

    private void AddCompetition()
    {
        if (SelectedTournament == null) return;
        var newCompetition = new Competition
        {
            CompetitionDate = NewCompetitionDate,
            CompetitionNumber = NewCompetitionNumber,
            Player1_RegNumber = NewP1_RegNumber,
            Player1_Result = NewP1_Result,
            Player1_Moves = NewP1_Moves,
            Player2_RegNumber = NewP2_RegNumber,
            Player2_Result = NewP2_Result,
            Player2_Moves = NewP2_Moves
        };

        SelectedTournament.Competitions.Add(newCompetition);
        
        OnPropertyChanged(nameof(Competitions));
    }

    private void majTournament(Tournament tournament)
    {
        repository.UpdateTournament(tournament);
    }
}

public class RelayCommand2 : ICommand
{
    private readonly Action<object?>? _executeWithParam;
    private readonly Action? _executeNoParam;

    public RelayCommand2(Action<object?> execute)
    {
        _executeWithParam = execute;
        _executeNoParam = null;
    }
    public RelayCommand2(Action execute)
    {
        _executeNoParam = execute;
        _executeWithParam = null;
    }

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