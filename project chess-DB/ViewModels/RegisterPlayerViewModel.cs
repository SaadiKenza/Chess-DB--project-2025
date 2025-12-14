using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Reactive;
using ReactiveUI;
using project_chess_DB.Services;
using Avalonia.Rendering.Composition;

namespace project_chess_DB.ViewModels;

public class RegisterPlayerViewModel : ReactiveObject
{
    private readonly ObservableCollection<string> _sourcePlayers;
    private readonly PlayerRepository _repository;
    private ObservableCollection<string> _filteredPlayers;
    public ObservableCollection<string> FilteredPlayers
    {
        get => _filteredPlayers;
        set => this.RaiseAndSetIfChanged(ref _filteredPlayers, value);
    }
    private string _newMatricule = string.Empty;
    public string NewMatricule
    {
        get => _newMatricule;
        set
        {
            this.RaiseAndSetIfChanged(ref _newMatricule, value);
            if (!string.IsNullOrEmpty(ErrorMessage)) ErrorMessage = string.Empty;
        }
    }
    private string _errorMessage = string.Empty;
    public string ErrorMessage
    {
        get => _errorMessage;
        set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
    }
    private string _searchText = string.Empty;
    public string SearchText
    {
        get => _searchText;
        set
        {
            this.RaiseAndSetIfChanged(ref _searchText, value);
            FilterList();
        }
    }


    public ReactiveCommand<Unit, Unit> AddPlayerCommand { get; }
    public ReactiveCommand<string, Unit> DeletePlayerCommand { get; }
    public ReactiveCommand<Unit, Unit> CloseCommand { get; }
    public Action? CloseAction { get; set; }
    public RegisterPlayerViewModel(ObservableCollection<string> tournamentPlayers)
    {
        _sourcePlayers = tournamentPlayers;
        _repository=new PlayerRepository();

        _filteredPlayers = new ObservableCollection<string>(_sourcePlayers);

        AddPlayerCommand = ReactiveCommand.Create(AddPlayer);
        DeletePlayerCommand = ReactiveCommand.Create<string>(DeletePlayer);
        CloseCommand = ReactiveCommand.Create(() => CloseAction?.Invoke());
    }

    private void AddPlayer()
    {
        if (string.IsNullOrWhiteSpace(NewMatricule)) return;
        if (_sourcePlayers.Contains(NewMatricule))
        {
            ErrorMessage="Joueur déjà inscrit au tournoi";
            return;
        }
        bool existsInDb=_repository.PlayerExists(NewMatricule);
        if (!existsInDb)
        {
            ErrorMessage="Matricule inconnu";
            return;
        } 
        _sourcePlayers.Add(NewMatricule);
        NewMatricule = string.Empty;
        ErrorMessage = string.Empty;
        FilterList();
    }

    private void DeletePlayer(string matricule)
    {
        if (_sourcePlayers.Contains(matricule))
        {
            _sourcePlayers.Remove(matricule);
            FilterList();
        }
    }

    private void FilterList()
    {
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            FilteredPlayers = new ObservableCollection<string>(_sourcePlayers);
        }
        else
        {
            var result = _sourcePlayers
                .Where(x => x.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                .ToList();
            FilteredPlayers = new ObservableCollection<string>(result);
        }
    }
}