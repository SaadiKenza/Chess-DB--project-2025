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

    private string _newName_of_the_tournament { get; set; } = string.Empty;
    private string _newCountry { get; set; } = string.Empty;
    private string _newCity { get; set; } = string.Empty;
    private string _newStart_date { get; set; } = string.Empty;
    private string _newEnd_date { get; set; } = string.Empty;
    public string NewName_of_the_tournament
    {
        get => _newName_of_the_tournament;
        set { if (_newName_of_the_tournament != value) { _newName_of_the_tournament = value; OnPropertyChanged(nameof(NewName_of_the_tournament)); (AddTournamentCommand as RelayCommand2)?.RaiseCanExecuteChanged(); } }
    }
    public string NewCountry
    {
        get => _newCountry;
        set { if (_newCountry != value) { _newCountry = value; OnPropertyChanged(nameof(NewCountry)); (AddTournamentCommand as RelayCommand2)?.RaiseCanExecuteChanged(); } }
    }
    public string NewCity
    {
        get => _newCity;
        set { if (_newCity != value) { _newCity = value; OnPropertyChanged(nameof(NewCity)); (AddTournamentCommand as RelayCommand2)?.RaiseCanExecuteChanged(); } }
    }
    public string NewStart_date
    {
        get => _newStart_date;
        set { if (_newStart_date != value) { _newStart_date = value; OnPropertyChanged(nameof(NewStart_date)); (AddTournamentCommand as RelayCommand2)?.RaiseCanExecuteChanged(); } }
    }
    public string NewEnd_date
    {
        get => _newEnd_date;
        set { if (_newEnd_date != value) { _newEnd_date = value; OnPropertyChanged(nameof(NewEnd_date)); (AddTournamentCommand as RelayCommand2)?.RaiseCanExecuteChanged(); } }
    }


    private string _newCompetitionDate { get; set; } = string.Empty;
    private string _newCompetitionNumber { get; set; } = string.Empty;
    public string NewCompetitionDate
    {
        get => _newCompetitionDate;
        set { if (_newCompetitionDate != value) { _newCompetitionDate = value; OnPropertyChanged(nameof(NewCompetitionDate)); (AddCompetitionCommand as RelayCommand2)?.RaiseCanExecuteChanged(); } }
    }
    public string NewCompetitionNumber
    {
        get => _newCompetitionNumber;
        set { if (_newCompetitionNumber != value) { _newCompetitionNumber = value; OnPropertyChanged(nameof(NewCompetitionNumber)); (AddCompetitionCommand as RelayCommand2)?.RaiseCanExecuteChanged(); } }
    }
    private string _newP1_RegNumber { get; set; } = string.Empty;
    private string _newP1_Result { get; set; } = string.Empty;
    private string _newP1_Moves { get; set; } = string.Empty;
    public string NewP1_RegNumber
    {
        get => _newP1_RegNumber;
        set { if (_newP1_RegNumber != value) { _newP1_RegNumber = value; OnPropertyChanged(nameof(NewP1_RegNumber)); (AddCompetitionCommand as RelayCommand2)?.RaiseCanExecuteChanged(); } }
    }
    public string NewP1_Result
    {
        get => _newP1_Result;
        set { if (_newP1_Result != value) { _newP1_Result = value; OnPropertyChanged(nameof(NewP1_Result)); (AddCompetitionCommand as RelayCommand2)?.RaiseCanExecuteChanged(); } }
    }
    public string NewP1_Moves
    {
        get => _newP1_Moves;
        set { if (_newP1_Moves != value) { _newP1_Moves = value; OnPropertyChanged(nameof(NewP1_Moves)); (AddCompetitionCommand as RelayCommand2)?.RaiseCanExecuteChanged(); } }
    }
    private string _newP2_RegNumber { get; set; } = string.Empty;
    private string _newP2_Result { get; set; } = string.Empty;
    private string _newP2_Moves { get; set; } = string.Empty;
    public string NewP2_RegNumber
    {
        get => _newP2_RegNumber;
        set { if (_newP2_RegNumber != value) { _newP2_RegNumber = value; OnPropertyChanged(nameof(NewP2_RegNumber)); (AddCompetitionCommand as RelayCommand2)?.RaiseCanExecuteChanged(); } }
    }
    public string NewP2_Result
    {
        get => _newP2_Result;
        set { if (_newP2_Result != value) { _newP2_Result = value; OnPropertyChanged(nameof(NewP2_Result)); (AddCompetitionCommand as RelayCommand2)?.RaiseCanExecuteChanged(); } }
    }
    public string NewP2_Moves
    {
        get => _newP2_Moves;
        set { if (_newP2_Moves != value) { _newP2_Moves = value; OnPropertyChanged(nameof(NewP2_Moves)); (AddCompetitionCommand as RelayCommand2)?.RaiseCanExecuteChanged(); } }
    }
    public ICommand AddTournamentCommand { get; }
    public ICommand DeleteTournamentCommand { get; }
    public ICommand OpenJoinTournamentCommand { get; }
    public ICommand AddCompetitionCommand { get; }

    private TournamentRepository repository;
    private List<Tournament> _allTournaments;
    private string _searchText = string.Empty;
    public string SearchText
    {
        get => _searchText;
        set
        {
            if (_searchText != value)
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));

                FilterTournaments();
            }
        }
    }
    public TournamentPageViewModel()
    {
        repository = new TournamentRepository();
        _allTournaments = repository.GetAllTournaments();
        Tournaments = new ObservableCollection<Tournament>(_allTournaments);

        OpenJoinTournamentCommand = ReactiveCommand.CreateFromTask<Tournament>(OpenRegistrationWindow, outputScheduler: RxApp.MainThreadScheduler);
        AddTournamentCommand = new RelayCommand2(AddTournament, CanAddTournament);
        DeleteTournamentCommand = new RelayCommand2(DeleteTournament);
        AddCompetitionCommand = new RelayCommand2(AddCompetition, CanAddCompetition);
    }
    private void FilterTournaments()
    {
        // Si la recherche est vide, on remet tout le monde
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            // On remet la liste originale
            Tournaments = new ObservableCollection<Tournament>(_allTournaments);
        }
        else
        {
            var filteredList = _allTournaments
                .Where(p => p.Name_of_the_tournament != null && p.Name_of_the_tournament.StartsWith(SearchText))
                .ToList();

            Tournaments = new ObservableCollection<Tournament>(filteredList);
        }
        OnPropertyChanged(nameof(Tournaments));
    }
    private async Task OpenRegistrationWindow(Tournament tournament)
    {
        if (tournament == null) return;

        // Sécurité : on s'assure que la liste existe
        if (tournament.RegisteredPlayers == null)
            tournament.RegisteredPlayers = new ObservableCollection<string>();

        // On exécute tout sur le thread UI
        await Dispatcher.UIThread.InvokeAsync(async () =>
        {
            // 1. Création du ViewModel avec les données du tournoi
            var registerVm = new RegisterPlayerViewModel(tournament.RegisteredPlayers);

            // 2. Création de la Vue (Fenêtre)
            var dialog = new RegisterPlayerPageView();

            // 3. IMPORTANT : Lier le ViewModel à la Vue
            dialog.DataContext = registerVm;

            // 4. Afficher la fenêtre
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var mainWindow = desktop.MainWindow;
                if (mainWindow is not null)
                {
                    // On attend que la fenêtre se ferme
                    await dialog.ShowDialog(mainWindow);
                }
            }
        });
    }
    private bool CanAddTournament()
    {
        return !string.IsNullOrWhiteSpace(NewName_of_the_tournament) &&
               !string.IsNullOrWhiteSpace(NewCountry) &&
               !string.IsNullOrWhiteSpace(NewCity) &&
               !string.IsNullOrWhiteSpace(NewStart_date) &&
               !string.IsNullOrWhiteSpace(NewEnd_date);
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
        _allTournaments.Add(newTournament);
        FilterTournaments();
        ClearForm();
    }
    private void ClearForm()
    {
        NewName_of_the_tournament = string.Empty;
        NewCountry = string.Empty;
        NewCity = string.Empty;
        NewStart_date = string.Empty;
        NewEnd_date = string.Empty;
    }

    private void DeleteTournament(object? parameter)
    {
        if (parameter is Tournament tournamentToDelete)
        {
            repository.DeleteTournament(tournamentToDelete.Name_of_the_tournament);
            _allTournaments.Remove(tournamentToDelete);
            FilterTournaments();

            if (SelectedTournament == tournamentToDelete)
            {
                SelectedTournament = null;
            }
        }
    }
    private bool CanAddCompetition()
    {
        return !string.IsNullOrWhiteSpace(NewCompetitionDate) &&
               !string.IsNullOrWhiteSpace(NewCompetitionNumber) &&
               !string.IsNullOrWhiteSpace(NewP1_RegNumber) &&
               !string.IsNullOrWhiteSpace(NewP1_Result) &&
               !string.IsNullOrWhiteSpace(NewP1_Moves) &&
               !string.IsNullOrWhiteSpace(NewP2_RegNumber) &&
               !string.IsNullOrWhiteSpace(NewP2_Result) &&
               !string.IsNullOrWhiteSpace(NewP2_Moves);
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
        ClearCompetitionForm();
    }
    private void ClearCompetitionForm()
    {
        NewCompetitionDate = string.Empty;
        NewCompetitionNumber = string.Empty;

        // Joueur 1
        NewP1_RegNumber = string.Empty;
        NewP1_Result = string.Empty;
        NewP1_Moves = string.Empty;

        // Joueur 2
        NewP2_RegNumber = string.Empty;
        NewP2_Result = string.Empty;
        NewP2_Moves = string.Empty;
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
    private readonly Predicate<object?>? _canExecute;
    public RelayCommand2(Action<object?> execute, Predicate<object?>? canExecute = null)
    {
        _executeWithParam = execute;
        _executeNoParam = null;
        _canExecute = canExecute;
    }

    public RelayCommand2(Action execute, Func<bool>? canExecute = null)
    {
        _executeNoParam = execute;
        _executeWithParam = null;
        if (canExecute != null)
        {
            _canExecute = _ => canExecute();
        }
    }
    public event EventHandler? CanExecuteChanged;
    public bool CanExecute(object? parameter)
    {
        return _canExecute == null || _canExecute(parameter);
    }

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
    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}