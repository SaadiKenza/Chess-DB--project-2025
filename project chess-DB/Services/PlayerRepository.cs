using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using project_chess_DB.Models;

//Joueur : Player (le ficier models), Service : PlayerRepository , table sql (db): Players, un joueur à ajouter player 
//player est un objet de Player

namespace project_chess_DB.Services
{
    public class PlayerRepository
    {
        public void AddPlayer(Player player)
        {
            using var connection = DatabaseService.GetOpenConnection();

            var sql = @"
                INSERT INTO Players
                    (Matricule, Last_name, First_name, Age, Elo, Country, Mail, Phone_number)
                VALUES
                    ($matricule, $lastName, $firstName, $age, $elo, $country, $mail, $phone)
            ";

            using var command = connection.CreateCommand();
            command.CommandText = sql;

            command.Parameters.AddWithValue("$matricule", player.Matricule);
            command.Parameters.AddWithValue("$lastName", player.Last_name);
            command.Parameters.AddWithValue("$firstName", player.First_name);
            command.Parameters.AddWithValue("$age", player.Age);
            command.Parameters.AddWithValue("$elo", player.Elo);
            command.Parameters.AddWithValue("$country", player.Country);
            command.Parameters.AddWithValue("$mail", player.Mail);
            command.Parameters.AddWithValue("$phone", player.Phone_number);

            command.ExecuteNonQuery();
        }

        public List<Player> GetAllPlayers() //prendre tous les joueurs depuis la base
        {
            var players = new List<Player>(); //liste qui contient les player

            using var connection = DatabaseService.GetOpenConnection();

            var sql = @" SELECT Matricule, Last_name, First_name, Age, Elo, Country, Mail, Phone_number FROM Players";

            using var command = connection.CreateCommand();
            command.CommandText = sql;

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var player = new Player
                (
                    reader.GetString(0), // Matricule
                    reader.GetString(1), // Last_name
                    reader.GetString(2), // First_name
                    reader.GetString(3), // Age
                    reader.GetString(4), // Elo
                    reader.GetString(5), // Country
                    reader.GetString(6), // Mail
                    reader.GetString(7)  // Phone_number
                );

                players.Add(player);
            }

            return players; //retourne la liste avec le nouveau player
        }

        // Tu peux ajouter UpdatePlayer, DeletePlayer, GetByMatricule, etc.
    }
}


