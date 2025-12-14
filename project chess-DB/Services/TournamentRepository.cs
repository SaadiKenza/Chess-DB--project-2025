using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using project_chess_DB.Models;

//Tournaments : table SQL, Tournament : le fichier models.cs, Name : la clé primaire, tournament: le nouveau qu'on crée/ajoute, tournaments : liste des tournament

namespace project_chess_DB.Services
{

    public class TournamentRepository
    {

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
        public void DeleteTournament(string name_of_the_tournament)
        {
            using var connection = DatabaseService.GetOpenConnection();
            var sql = @"DELETE FROM Tournaments WHERE Name_of_the_tournament = $name_of_the_tournament";

            using var command5 = connection.CreateCommand();
            command5.CommandText = sql;
            command5.Parameters.AddWithValue("$name_of_the_tournament", name_of_the_tournament);
            command5.ExecuteNonQuery();
        }
        public void UpdateTournament(Tournament tournament)
        {
            using var connection = DatabaseService.GetOpenConnection();
            var sql = @"UPDATE Tournaments SET
            Country = $country,
            City = $city,
            Start_date = $start_date,
            End_date = $end_date
            WHERE Name_of_the_tournament = $name_of_the_tournament";
            using var command6 = connection.CreateCommand();
            command6.CommandText = sql;
            command6.Parameters.AddWithValue("$name_of_the_tournament", tournament.Name_of_the_tournament);
            command6.Parameters.AddWithValue("$country", tournament.Country);
            command6.Parameters.AddWithValue("$city", tournament.City);
            command6.Parameters.AddWithValue("$start_date", tournament.Start_date);
            command6.Parameters.AddWithValue("$end_date", tournament.End_date);

            command6.ExecuteNonQuery();
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
                string rawStart = reader.IsDBNull(3) ? "" : reader.GetString(3);
                string rawEnd = reader.IsDBNull(4) ? "" : reader.GetString(4);
                string cleanStart = CleanDate(rawStart);
                string cleanEnd = CleanDate(rawEnd);
                var tournament = new Tournament
                (
                    reader.GetString(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    cleanStart,
                    cleanEnd
                );

                tournaments.Add(tournament);
            }

            return tournaments;
        }
        private string CleanDate(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "";
            if (input.Contains(" "))
            {
                string[] parts = input.Split(' ');
                if (DateTime.TryParse(parts[0], out DateTime dt))
                {
                    return dt.ToString("dd/MM/yyyy");
                }
                return parts[0]; 
            }
            if (DateTime.TryParse(input, out DateTime date))
            {
                return date.ToString("dd/MM/yyyy");
            }

            return input;
        }

    }
}