using project_chess_DB.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System;
using System.Linq;
using Tmds.DBus.Protocol;
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

    public string NewName_of_the_tournament { get; set; } = string.Empty;
    public string NewCountry { get; set; } = string.Empty;
    public string NewCity { get; set; } = string.Empty;
    public string NewStart_date { get; set; } = string.Empty;
    public string NewEnd_date { get; set; } = string.Empty;
    public ICommand AddTournamentCommand { get; }
    public ICommand DeleteTournamentCommand { get; }

    private TournamentRepository repository;
    public ICommand OpenJoinTournamentCommand { get; }
    public TournamentPageViewModel()
    {
        // Juste pour vérifier si le ViewModel est bien créé
        //System.Diagnostics.Debug.WriteLine("✅ TournamentPageViewModel CONSTRUIT");
        OpenJoinTournamentCommand = ReactiveCommand.CreateFromTask<Tournament>(OpenRegistrationWindow, outputScheduler: RxApp.MainThreadScheduler);
        repository = new TournamentRepository();
        var tournamentFromDb = repository.GetAllTournaments();
        Tournaments = new ObservableCollection<Tournament>(tournamentFromDb);

        // On garde tes commandes comme avant
        AddTournamentCommand = new RelayCommand2(AddTournament);
        DeleteTournamentCommand = new RelayCommand2(DeleteTournament);
    }
    private async Task OpenRegistrationWindow(Tournament tournament)
    {
        if (tournament == null) return;

        // On utilise Dispatcher.UIThread.InvokeAsync pour garantir qu'on touche à l'UI sur le bon thread
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
            //pour supprimer dans la database
            repository.DeleteTournament(tournamentToDelete.Name_of_the_tournament);
            //supprimer dans l'UI
            Tournaments.Remove(tournamentToDelete);
        }
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

    public RelayCommand2(Action<object?> execute)   //avec paramètre
    {
        _executeWithParam = execute;
        _executeNoParam = null;
    }
    public RelayCommand2(Action execute)        //code quand on veut seullement ajouter
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