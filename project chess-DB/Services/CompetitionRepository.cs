using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using project_chess_DB.Models;

namespace project_chess_DB.Services
{
    public class CompetitionRepository
    {

        public List<Competition> GetCompetitionsForTournament(string tournamentName)
        {
            var competitions = new List<Competition>();

            using var connection = DatabaseService.GetOpenConnection();
            using var command = connection.CreateCommand();

            command.CommandText = @"
                SELECT CompetitionDate, CompetitionNumber,
                       Player1Matricule, Player1Result, Player1Moves,
                       Player2Matricule, Player2Result, Player2Moves
                FROM Competitions
                WHERE TournamentName = @tournamentName
                ORDER BY CompetitionNumber;
            ";

            command.Parameters.AddWithValue("@tournamentName", tournamentName);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var competition = new Competition(

                reader.GetString(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetString(3),
                reader.GetString(4),
                reader.GetString(5),
                reader.GetString(6),
                reader.GetString(7)
                );
                competitions.Add(competition);
            }


            return competitions;
        }


        public bool AddCompetition(string tournamentName, Competition competition)
        {
            using var connection = DatabaseService.GetOpenConnection();
            using var command = connection.CreateCommand();

            command.CommandText = @"
                INSERT INTO Competitions
                (TournamentName, CompetitionNumber, CompetitionDate,
                 Player1Matricule, Player1Result, Player1Moves,
                 Player2Matricule, Player2Result, Player2Moves)
                VALUES
                (@t, @num, @date, @p1, @r1, @m1, @p2, @r2, @m2);
            ";

            command.Parameters.AddWithValue("@t", tournamentName);
            command.Parameters.AddWithValue("@num", competition.CompetitionNumber);
            command.Parameters.AddWithValue("@date", competition.CompetitionDate);

            command.Parameters.AddWithValue("@p1", competition.Player1_RegNumber);
            command.Parameters.AddWithValue("@r1", competition.Player1_Result);
            command.Parameters.AddWithValue("@m1", competition.Player1_Moves);

            command.Parameters.AddWithValue("@p2", competition.Player2_RegNumber);
            command.Parameters.AddWithValue("@r2", competition.Player2_Result);
            command.Parameters.AddWithValue("@m2", competition.Player2_Moves);

            return command.ExecuteNonQuery() > 0;
        }


        public void DeleteCompetition(string tournamentName, string competitionNumber)
        {
            using var connection = DatabaseService.GetOpenConnection();
            using var command = connection.CreateCommand();

            command.CommandText = @"
                DELETE FROM Competitions
                WHERE TournamentName = @tournamentName
                AND CompetitionNumber = @competitionNumber;
            ";

            command.Parameters.AddWithValue("@tournamentName", tournamentName);
            command.Parameters.AddWithValue("@competitionNumber", competitionNumber);

            command.ExecuteNonQuery();
        }
    }
}
