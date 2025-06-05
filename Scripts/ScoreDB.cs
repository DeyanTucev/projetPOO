using Godot;
using Microsoft.Data.Sqlite;
using System.IO;

public partial class ScoreDB : Node
{
    private string dbPath;

    public override void _Ready()
    {
        dbPath = ProjectSettings.GlobalizePath("../scores.db");
        CreateDatabaseIfNeeded();

    }

    private void CreateDatabaseIfNeeded()
    {
        if (!File.Exists(dbPath))
        {
            using var conn = new SqliteConnection($"Data Source={dbPath}");
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Scores (
                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                    playerName TEXT NOT NULL,
                    score INTEGER NOT NULL
                )";
            cmd.ExecuteNonQuery();
        }
    }

    public void SaveScore(string playerName, int score)
    {
        using var conn = new SqliteConnection($"Data Source={dbPath}");
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = @"INSERT INTO Scores (playerName, score) VALUES ($username, $score)";
        cmd.Parameters.AddWithValue("$username", playerName);
        cmd.Parameters.AddWithValue("$score", score);
        cmd.ExecuteNonQuery();
    }
        
}