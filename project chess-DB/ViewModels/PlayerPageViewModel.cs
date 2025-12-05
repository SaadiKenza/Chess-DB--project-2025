using project_chess_DB.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System;
using System.Linq;
namespace project_chess_DB.ViewModels;

public partial class PlayerPageViewModel : ViewModelBase
{
    public ObservableCollection<Player> Players { get; set; }

    public string NewLast_name { get; set; } = string.Empty;
    public string NewFirst_name { get; set; } = string.Empty;
    public string NewAge { get; set; } = string.Empty;
    public string NewElo { get; set; } = string.Empty;
    public string NewCountry { get; set; } = string.Empty;
    public string NewMail { get; set; } = string.Empty;
    public string NewPhone_number { get; set; } = string.Empty;
    public ICommand AddPlayerCommand { get; }   //commande pour notre bouton add player
    public ICommand DeletePlayerCommand { get; }

    public PlayerPageViewModel()
    {
        var players = new List<Player>
        {

        };
        Players = new ObservableCollection<Player>(players);
        AddPlayerCommand = new RelayCommand(AddPlayer);
        DeletePlayerCommand = new RelayCommand(DeletePlayer);
    }
    private void AddPlayer()
    {
        string matriculeGenere = GenerateMatricule();        //genere le matricule automatique
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
        Players.Add(newPlayer);
    }
    private void DeletePlayer(object? parameter)
    {
        if (parameter is Player playerToDelete)
        {
            Players.Remove(playerToDelete);
        }
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

    public RelayCommand(Action<object?> execute)   //avec paramètre
    {
        _executeWithParam = execute;
        _executeNoParam = null;
    }
    public RelayCommand(Action execute)        //code quand on veut seullement ajouter
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