using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using project_chess_DB.Models;

//Tournaments : table SQL, Tournament : le fichier models.cs, Name : la clé primaire, tournament: le nouveau qu'on crée/ajoute, tournaments : liste des tournament

namespace project_chess_DB.Services{

    public class TournamentRepository{

        public void AddTournament(Tournament tournament)
        {
            using var connection = DatabaseService.GetOpenConnection();

            var sql = @"
                INSERT INTO Tournaments (Name_of_the_tournament, Country,City, Start_date, End_date)
                VALUES ($name_of_the_tournament, $country, $city, $start_date, $end_date)
            ";

            using var command2 = connection.CreateCommand();
            command2.CommandText = sql;

            command2.Parameters.AddWithValue("$name_of_the_tournament", tournament.Name_of_the_tournament);
            command2.Parameters.AddWithValue("$country", tournament.Country);
            command2.Parameters.AddWithValue("$city", tournament.City);
            command2.Parameters.AddWithValue("$start_date", tournament.Start_date);
            command2.Parameters.AddWithValue("$end_date", tournament.End_date);
            

            command2.ExecuteNonQuery();
        }
        public void DeleteTournament (string name_of_the_tournament)
        {
            using var connection = DatabaseService.GetOpenConnection();
            var sql = @"DELETE FROM Tournaments WHERE Name_of_the_tournament = $name_of_the_tournament";

            using var command5 = connection.CreateCommand();
            command5.CommandText = sql;
            command5.Parameters.AddWithValue("$name_of_the_tournament", name_of_the_tournament);
            command5.ExecuteNonQuery();
        }

        public List<Tournament> GetAllTournaments()
        {
            var tournaments = new List<Tournament>();

            using var connection = DatabaseService.GetOpenConnection();

            var sql = @"SELECT Name_of_the_tournament, Country, City, Start_date, End_date FROM Tournaments";

            using var command2 = connection.CreateCommand();
            command2.CommandText = sql;

            using var reader = command2.ExecuteReader();

            while (reader.Read())
            {
                var tournament = new Tournament
                (
                    reader.GetString(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4)
                );

                tournaments.Add(tournament);
            }

            return tournaments;
        }

    }
}