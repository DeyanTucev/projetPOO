using Godot;
using Microsoft.Data.Sqlite;
using System.IO;

public partial class ScoreDB : Node
{
	private string dbPath;

	public override void _Ready()
	{
		dbPath = ProjectSettings.GlobalizePath("./scores.db");
		CreateDatabaseIfNeeded();
		GD.Print("DB path: " + dbPath);

	}

	private void CreateDatabaseIfNeeded()
	{
		if (!File.Exists(dbPath))
		{
			using var conn = new SqliteConnection($"Data Source={dbPath}");
			conn.Open();
			var cmd = conn.CreateCommand();
			cmd.CommandText = @"
				CREATE TABLE IF NOT EXISTS playerScore (
					ID INTEGER PRIMARY KEY AUTOINCREMENT,
					Pseudo TEXT NOT NULL,
					Score INTEGER NOT NULL
				)";
			cmd.ExecuteNonQuery();
		}
	}
		
}
