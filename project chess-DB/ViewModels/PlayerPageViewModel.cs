using project_chess_DB.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System;
using System.Linq;
using project_chess_DB.Services;
using Tmds.DBus.Protocol;
using Avalonia.Controls;

namespace project_chess_DB.ViewModels;

public partial class PlayerPageViewModel : ViewModelBase
{
    public ObservableCollection<Player> Players { get; set; }

    private string _newLast_name = string.Empty;
    private string _newFirst_name = string.Empty;
    private string _newAge = string.Empty;
    private string _newElo = string.Empty;
    private string _newCountry = string.Empty;
    private string _newMail = string.Empty;
    private string _newPhone_number = string.Empty;
    public string NewLast_name
    {
        get => _newLast_name;
        set
        {
            // On vérifie si la valeur change pour éviter des mises à jour inutiles
            if (_newLast_name != value)
            {
                _newLast_name = value;
                // IMPORTANT : Ceci dit à la vue "Hey, j'ai changé, mets-toi à jour !"
                OnPropertyChanged(nameof(NewLast_name));
                (AddPlayerCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }
    }

    public string NewFirst_name
    {
        get => _newFirst_name;
        set
        {
            if (_newFirst_name != value)
            {
                _newFirst_name = value;
                OnPropertyChanged(nameof(NewFirst_name));
                (AddPlayerCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }
    }

    // ... Fais de même pour NewAge, NewElo, NewCountry, NewMail, NewPhone_number
    public string NewAge
    {
        get => _newAge;
        set { if (_newAge != value) { _newAge = value; OnPropertyChanged(nameof(NewAge)); (AddPlayerCommand as RelayCommand)?.RaiseCanExecuteChanged(); } }
    }
    public string NewElo
    {
        get => _newElo;
        set { if (_newElo != value) { _newElo = value; OnPropertyChanged(nameof(NewElo)); (AddPlayerCommand as RelayCommand)?.RaiseCanExecuteChanged(); } }
    }
    public string NewCountry
    {
        get => _newCountry;
        set { if (_newCountry != value) { _newCountry = value; OnPropertyChanged(nameof(NewCountry)); (AddPlayerCommand as RelayCommand)?.RaiseCanExecuteChanged(); } }
    }
    public string NewMail
    {
        get => _newMail;
        set { if (_newMail != value) { _newMail = value; OnPropertyChanged(nameof(NewMail)); (AddPlayerCommand as RelayCommand)?.RaiseCanExecuteChanged(); } }
    }
    public string NewPhone_number
    {
        get => _newPhone_number;
        set { if (_newPhone_number != value) { _newPhone_number = value; OnPropertyChanged(nameof(NewPhone_number)); (AddPlayerCommand as RelayCommand)?.RaiseCanExecuteChanged(); } }
    }
    public ICommand AddPlayerCommand { get; }   //commande pour notre bouton add player
    public ICommand DeletePlayerCommand { get; }

    private PlayerRepository repository;
    public PlayerPageViewModel()
    {
        repository = new PlayerRepository();
        var playersFromDb = repository.GetAllPlayers();
        Players = new ObservableCollection<Player>(playersFromDb);
        AddPlayerCommand = new RelayCommand(AddPlayer, CanAddPlayer);
        DeletePlayerCommand = new RelayCommand(DeletePlayer);
    }
    private bool CanAddPlayer()
    {
        // On retourne VRAI seulement si AUCUN champ n'est vide ou nul
        return !string.IsNullOrWhiteSpace(NewLast_name) &&
               !string.IsNullOrWhiteSpace(NewFirst_name) &&
               !string.IsNullOrWhiteSpace(NewAge) &&
               !string.IsNullOrWhiteSpace(NewElo) &&
               !string.IsNullOrWhiteSpace(NewCountry) &&
               !string.IsNullOrWhiteSpace(NewMail) &&
               !string.IsNullOrWhiteSpace(NewPhone_number);
    }
    private void AddPlayer()
    {
        string matriculeGenere = GenerateMatricule(); //genere le matricule automatique
        //on crée notre nouveau joueur avec les valeurs entrées dans les textbox
        var newPlayer = new Player(
            matriculeGenere,
            NewLast_name,
            NewFirst_name,
            NewAge,
            NewElo,
            NewCountry,
            NewMail,
            NewPhone_number

        );
        repository.AddPlayer(newPlayer);
        Players.Add(newPlayer);
        ClearForm();

    }
    private void ClearForm()
    {
        NewLast_name = string.Empty;
        NewFirst_name = string.Empty;
        NewAge = string.Empty;
        NewElo = string.Empty;
        NewCountry = string.Empty;
        NewMail = string.Empty;
        NewPhone_number = string.Empty;
    }
    private void DeletePlayer(object? parameter)
    {
        if (parameter is Player playerToDelete)
        {
            //pour supprimer dans la database
            repository.DeletePlayer(playerToDelete.Matricule);
            //supprimer dans l'UI
            Players.Remove(playerToDelete);
        }
    }
    private void majPlayer(Player player)
    {
        repository.UpdatePlayer(player);
    }
    private string GenerateMatricule()
    {
        // 1. On récupère l'année (ex: "25")
        string currentYear = DateTime.Now.ToString("yy");

        // 2. On cherche TOUS les matricules existants qui commencent par "25"
        var existingMatricules = Players
            .Where(p => p.Matricule != null && p.Matricule.StartsWith(currentYear))
            .Select(p => p.Matricule)
            .ToList();

        // 3. S'il n'y en a aucun, on commence à 1 (donc 25001)
        if (!existingMatricules.Any())
        {
            return $"{currentYear}001";
        }

        // 4. Sinon, on cherche le numéro le plus élevé
        int maxSequence = existingMatricules
            .Select(m =>
            {
                // On enlève le "25" du début pour garder juste le nombre (ex: "25002" -> "002")
                string numberPart = m.Substring(2);

                // On essaie de convertir en chiffre
                if (int.TryParse(numberPart, out int number))
                {
                    return number;
                }
                return 0; // Si le matricule est bizarre, on l'ignore
            })
            .Max();

        // 5. On ajoute 1 au maximum trouvé
        return $"{currentYear}{(maxSequence + 1):D3}";
    }
}
public class RelayCommand : ICommand
{
    private readonly Action<object?>? _executeWithParam;
    private readonly Action? _executeNoParam;
    private readonly Predicate<object?>? _canExecute;
    public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute = null)
    {
        _executeWithParam = execute;
        _executeNoParam = null;
        _canExecute = canExecute;
    }

    public RelayCommand(Action execute, Func<bool>? canExecute = null)
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
