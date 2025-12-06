using System.IO;
using Microsoft.Data.Sqlite;
using System;
namespace project_chess_DB.Services
{
    public static class DatabaseService
    {
        
        private static readonly string DatabaseFolder =
            Path.Combine(AppContext.BaseDirectory, "Data");

        private static readonly string DatabasePath =
            Path.Combine(DatabaseFolder, "chess.db");

        private static readonly string ConnectionString =
            $"Data Source={DatabasePath}";
        // Chemin de la base de données (relatif à l'exécutable)
        /*private const string DatabasePath = "Data/chess.db";
        private const string DatabaseFolder = "Data";

 */
        /// <summary>
        /// Appelé au démarrage de l'appli pour s'assurer que
        /// le dossier Data existe et que les tables existent.
        /// </summary>
        public static void Initialize()
        {
            // 1. S'assurer que le dossier Data existe
            if (!Directory.Exists(DatabaseFolder))
            {
                Directory.CreateDirectory(DatabaseFolder);
            }

            // 2. Ouvrir une connexion -> si chess.db n'existe pas, SQLite le crée
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            // 3. Créer les tables si besoin
            CreateTables(connection);
        }

        private static void CreateTables(SqliteConnection connection)
        {
            // Ex : table Players
            var createPlayersTableSql = @"
                CREATE TABLE IF NOT EXISTS Players (
                Matricule TEXT PRIMARY KEY,
                Last_name TEXT NOT NULL,
                First_name TEXT NOT NULL,
                Age TEXT,
                Elo TEXT,
                Country TEXT,
                Mail TEXT,
                Phone_number TEXT
            );
            ";

            using var command = connection.CreateCommand();
            command.CommandText = createPlayersTableSql;
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Renvoie une connexion ouverte (à utiliser dans les repositories).
        /// </summary>
        public static SqliteConnection GetOpenConnection()
        {
            var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            return connection;
        }
    }
}
