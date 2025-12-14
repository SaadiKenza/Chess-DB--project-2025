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
            );" ;

            var createTournamentsTableSql = @"
            CREATE TABLE IF NOT EXISTS Tournaments (
                Name_of_the_tournament TEXT PRIMARY KEY,
                Country TEXT NOT NULL,
                City TEXT NOT NULL,
                Start_date TEXT NOT NULL,
                End_date TEXT NOT NULL
            );";

            var createTournamentPlayersTableSql = @"CREATE TABLE IF NOT EXISTS TournamentPlayers (
            TournamentName TEXT NOT NULL,
            PlayerMatricule TEXT NOT NULL,
            PRIMARY KEY (TournamentName, PlayerMatricule),
            FOREIGN KEY (TournamentName) REFERENCES Tournaments(Name_of_the_tournament),
            FOREIGN KEY (PlayerMatricule) REFERENCES Players (Matricule)
            )"; //les foreign key ici garantissent que le tournois existe dans Tournaments, de même pour player

            var createCompetitionsTableSql = @"CREATE TABLE IF NOT EXISTS Competitions (
            TournamentName TEXT NOT NULL,
            CompetitionNumber TEXT NOT NULL,
            CompetitionDate TEXT NOT NULL,
            Player1Matricule TEXT NOT NULL,
            Player2Matricule TEXT NOT NULL,
            Player1Result TEXT,
            Player2Result TEXT,
            Player1Moves TEXT,
            Player2Moves TEXT,

            PRIMARY KEY (TournamentName, CompetitionNumber),
            FOREIGN KEY (TournamentName) REFERENCES Tournaments(Name_of_the_tournament),
            FOREIGN KEY (Player1Matricule) REFERENCES Players(Matricule),
            FOREIGN KEY (Player2Matricule) REFERENCES Players(Matricule))";

            using var command = connection.CreateCommand();
            command.CommandText = createPlayersTableSql; //execution de la commande sql en haut, pour créer la table
            command.ExecuteNonQuery(); //la commande ne retourne rien

            using var command2 = connection.CreateCommand();
            command2.CommandText = createTournamentsTableSql;
            command2.ExecuteNonQuery();

            using var command3 = connection.CreateCommand();
            command3.CommandText = createTournamentPlayersTableSql; 
            command3.ExecuteNonQuery();

            using var command7 = connection.CreateCommand();
            command7.CommandText = createCompetitionsTableSql;
            command7.ExecuteNonQuery();
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
