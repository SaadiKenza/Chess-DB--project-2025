using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using project_chess_DB.Models;
using System;

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

        // pour supprimer un player dans la database aussi, et non pas que dans l'UI
        public void DeletePlayer(string matricule)
        {
            using var connection = DatabaseService.GetOpenConnection();
            var sql = @"DELETE FROM Players WHERE Matricule = $matricule";

            using var command3 = connection.CreateCommand();
            command3.CommandText = sql;
            command3.Parameters.AddWithValue("$matricule", matricule);
            command3.ExecuteNonQuery();
        }

        //pour pouvoir modifier les coordonnées d'un player
        public void UpdatePlayer(Player player)
        {
            using var connection = DatabaseService.GetOpenConnection();
            var sql = @"UPDATE Players SET
            Last_name = $lastName,
            First_name = $firstName,
            Age = $age,
            Elo = $elo,
            Country = $country,
            Mail = $mail,
            Phone_number = $phone
            WHERE Matricule = $matricule";
            using var command4 = connection.CreateCommand();
            command4.CommandText = sql;
            command4.Parameters.AddWithValue("$matricule", player.Matricule);
            command4.Parameters.AddWithValue("$lastName", player.Last_name);
            command4.Parameters.AddWithValue("$firstName", player.First_name);
            command4.Parameters.AddWithValue("$age", player.Age);
            command4.Parameters.AddWithValue("$elo", player.Elo);
            command4.Parameters.AddWithValue("$country", player.Country);
            command4.Parameters.AddWithValue("$mail", player.Mail);
            command4.Parameters.AddWithValue("$phone", player.Phone_number);

            command4.ExecuteNonQuery();
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
        //on vérifie si le joueur existe pour l'inscription à des tournois
        public bool PlayerExists(string matricule)
        {
            using var connection = DatabaseService.GetOpenConnection();
            var sql = "SELECT COUNT(*) FROM Players WHERE Matricule = $matricule";

            using var command = connection.CreateCommand();
            command.CommandText = sql;
            command.Parameters.AddWithValue("$matricule", matricule);
            var count = Convert.ToInt32(command.ExecuteScalar());
            return count > 0;
        }
    }
    // Dans project_chess_DB.Services.PlayerRepository
}


