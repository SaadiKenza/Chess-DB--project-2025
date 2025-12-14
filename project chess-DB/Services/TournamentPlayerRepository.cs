using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace project_chess_DB.Services
{
    public class TournamentPlayersRepository
    {
        public List<string> GetPlayersForTournament(string tournamentName)
        {
            var players = new List<string>();

            using var connection = DatabaseService.GetOpenConnection();
            using var command = connection.CreateCommand();

            command.CommandText = @"
                SELECT PlayerMatricule
                FROM TournamentPlayers
                WHERE TournamentName = @tournamentName
                ORDER BY PlayerMatricule;
            ";

            command.Parameters.AddWithValue("@tournamentName", tournamentName);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                players.Add(reader.GetString(0));
            }

            return players;
        }

        public bool AddPlayerToTournament(string tournamentName, string matricule)
        {
            using var connection = DatabaseService.GetOpenConnection();
            using var command = connection.CreateCommand();

            command.CommandText = @"
                INSERT OR IGNORE INTO TournamentPlayers (TournamentName, PlayerMatricule)
                VALUES (@tournamentName, @matricule);
            ";

            command.Parameters.AddWithValue("@tournamentName", tournamentName);
            command.Parameters.AddWithValue("@matricule", matricule);

            return command.ExecuteNonQuery() > 0;
        }

        public void RemovePlayerFromTournament(string tournamentName, string matricule)
        {
            using var connection = DatabaseService.GetOpenConnection();
            using var command = connection.CreateCommand();

            command.CommandText = @"
                DELETE FROM TournamentPlayers
                WHERE TournamentName = @tournamentName
                AND PlayerMatricule = @matricule;
            ";

            command.Parameters.AddWithValue("@tournamentName", tournamentName);
            command.Parameters.AddWithValue("@matricule", matricule);

            command.ExecuteNonQuery();
        }
    }
}
