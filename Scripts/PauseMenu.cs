using Godot;
using System;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

public partial class PauseMenu : Node2D
{
	private Button pauseButton;
	private Timer retryTimer;
	private Control PauseBox;
	
	private int score;
	
	public override void _Ready()
	{
		
		pauseButton = GetNode<Button>("PauseButton");
		PauseBox = GetNode<Control>("CenterContainer/PauseBox");
		
		var endButton = GetNode<Button>("CenterContainer/PauseBox/endButton");
		if (endButton == null)
		{
			GD.PrintErr("Button quitter introuvable !");
		}
		else
		{
			GD.Print("Boutton quitter trouvé !");
			endButton.Pressed += EndGame;
			endButton.ProcessMode = Godot.Node.ProcessModeEnum.Always;
			PauseBox.ProcessMode = Godot.Node.ProcessModeEnum.Always;
		}
		
		PauseBox.Visible = false;
		
		if (pauseButton == null)
		{
			GD.PrintErr("pauseButton introuvable !");
			return;
		}
		GD.Print("pauseButton trouvé.");
		
		pauseButton.FocusMode = Control.FocusModeEnum.None;
		
		pauseButton.ProcessMode = Node.ProcessModeEnum.Always;
		
		pauseButton.Connect("pressed", new Callable(this, nameof(OnPausePressed)));
		GD.Print("Connexion au signal 'pressed' réussie.");
	}
	
	private void OnPausePressed()
	{
		bool isPaused = !GetTree().Paused;
		GetTree().Paused = isPaused;
		GD.Print(isPaused ? "Jeu en pause." : "Jeu repris.");
		GD.Print("État actuel : " + GetTree().Paused);
		PauseBox.Visible = isPaused;
	}
	
	public void EndGame()
	{
		GD.Print("EndGame appelé !");
		
		Global global = (Global)GetNode("/root/Global");
		string pseudo = global.Pseudo;
		score = global.Score;
		
		GD.Print(pseudo);
		
		try
		{
			string exeDir = Path.GetDirectoryName(OS.GetExecutablePath());
			string dbPath = ScoreDB.GetDBPath();
			
			using var connection = new SQLiteConnection($"Data Source={dbPath}");
			connection.Open();
			
			using var insertCommand = connection.CreateCommand();
			insertCommand.CommandText = @"
			INSERT INTO playerScore (Pseudo, Score)
			VALUES (@pseudo, @score)
			ON CONFLICT(Pseudo) DO UPDATE SET Score =
				CASE WHEN @score > Score THEN @score ELSE Score END;
			";
			insertCommand.Parameters.AddWithValue("@pseudo", pseudo);
			insertCommand.Parameters.AddWithValue("@score", score);
			insertCommand.ExecuteNonQuery();
			
			GD.Print("Score sauvgardé !");
		}
		catch (Exception ex)
		{
			GD.PrintErr("Erreur : " + ex.Message);
		}
		GetTree().Paused = false;
		GetTree().ChangeSceneToFile("res://Scenes/main_menu.tscn");
	}
}
