using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using ReactiveUI;
using project_chess_DB.Models;
using project_chess_DB.Services;

namespace project_chess_DB.ViewModels;

public class RegisterCompetitionViewModel : ReactiveObject
{
    private readonly CompetitionRepository _CompRepository;
    private readonly string _tournamentName;
    private readonly ObservableCollection<Competition> _sourceCompetitions;
    private ObservableCollection<Competition> _filteredCompetitions;
    public ObservableCollection<Competition> FilteredCompetitions
    {
        get => _filteredCompetitions;
        set => this.RaiseAndSetIfChanged(ref _filteredCompetitions, value);
    }

    // Champs saisis dans l’UI
    public string CompetitionDate { get; set; } = string.Empty;
    public string CompetitionNumber { get; set; } = string.Empty;
    public string Player1_RegNumber { get; set; } = string.Empty;
    public string Player1_Result { get; set; } = string.Empty;
    public string Player1_Moves { get; set; } = string.Empty;
    public string Player2_RegNumber { get; set; } = string.Empty;
    public string Player2_Result { get; set; } = string.Empty;
    public string Player2_Moves { get; set; } = string.Empty;

    public ReactiveCommand<Unit, Unit> AddCompetitionCommand { get; }
    public ReactiveCommand<Competition, Unit> DeleteCompetitionCommand { get; }
    public RegisterCompetitionViewModel(string tournamentName)
    {
        _tournamentName = tournamentName;
        _CompRepository = new CompetitionRepository();
        _sourceCompetitions = new ObservableCollection<Competition> (_CompRepository.GetCompetitionsForTournament(_tournamentName));

        _filteredCompetitions = new ObservableCollection<Competition>(_sourceCompetitions);

        AddCompetitionCommand = ReactiveCommand.Create(AddCompetition);
        DeleteCompetitionCommand = ReactiveCommand.Create<Competition>(DeleteCompetition);
    }

    private void AddCompetition()
    {
        var competition = new Competition
        {
            CompetitionDate = CompetitionDate,
            CompetitionNumber = CompetitionNumber,
            Player1_RegNumber = Player1_RegNumber,
            Player1_Result = Player1_Result,
            Player1_Moves = Player1_Moves,
            Player2_RegNumber = Player2_RegNumber,
            Player2_Result = Player2_Result,
            Player2_Moves = Player2_Moves
        };

        _CompRepository.AddCompetition(_tournamentName, competition);
         _sourceCompetitions.Add(competition);
            
        //reset champs
        CompetitionDate = "";
        CompetitionNumber = "";
        Player1_RegNumber = "";
        Player1_Result = "";
        Player1_Moves = "";
        Player2_RegNumber = "";
        Player2_Result = "";
        Player2_Moves = "";
    }

    private void DeleteCompetition(Competition competition)
    {
    // 1. Supprime dans la DB
    _CompRepository.DeleteCompetition(
        _tournamentName,
        competition.CompetitionNumber
    );

    // 2. Supprime sans l'UI
    _sourceCompetitions.Remove(competition);

    }
}
