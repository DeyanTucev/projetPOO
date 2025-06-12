using Godot;
using System;
using System.Data.SQLite;
using System.IO;
using System.Text;

public partial class MainMenu : Control
{
	private Control settingsMenu;
	private Control creditsMenu;
	private Control mainButtons;
	private Control highScoreBox;
	
	private LineEdit pseudoInput;
	private HSlider volumeSlider;
	private HSlider musicSlider;
	private HSlider sfxSlider;
	private CheckBox fullScreenToggle;
	
	public override void _Ready()
	{
		settingsMenu = GetNode<Control>("CenterContainer/SettingsMenu");
		creditsMenu = GetNode<Control>("CenterContainer/CreditsMenu");
		mainButtons = GetNode<Control>("CenterContainer/MainButtons");
		highScoreBox = GetNode<Control>("CenterContainer/HighScoreBox");
		
		pseudoInput = GetNode<LineEdit>("Pseudo");
		
		volumeSlider = GetNode<HSlider>("CenterContainer/SettingsMenu/mainvolslider");
		musicSlider = GetNode<HSlider>("CenterContainer/SettingsMenu/musicvolslider");
		sfxSlider = GetNode<HSlider>("CenterContainer/SettingsMenu/sfxvolslider");
		fullScreenToggle = GetNode<CheckBox>("CenterContainer/SettingsMenu/fullscreen");
		
		GetNode<Button>("CenterContainer/SettingsMenu/back").Pressed += CloseSettings;
		GetNode<Button>("CenterContainer/CreditsMenu/back").Pressed += CloseCredits;
		GetNode<Button>("CenterContainer/HighScoreBox/back").Pressed += CloseScores;
		
		pseudoInput.Visible = true;
		settingsMenu.Visible = false;
		creditsMenu.Visible = false;
		highScoreBox.Visible = false;
		
		GetNode<Button>("CenterContainer/MainButtons/play").Pressed += OnPlayPressed;
		GetNode<Button>("CenterContainer/MainButtons/settings").Pressed += OnSettingsPressed;
		GetNode<Button>("CenterContainer/MainButtons/credits").Pressed += OnCreditsPressed;
		GetNode<Button>("CenterContainer/MainButtons/quit").Pressed += OnQuitPressed;
		GetNode<Button>("CenterContainer/MainButtons/highscore").Pressed += LoadHighScore;
	}
	
	private void OnPlayPressed()
	{
		string pseudo = pseudoInput.Text;
		if (pseudo != "")
		{
			GD.Print("Pseudo du joueur : " + pseudo);
		
			Global global = (Global)GetNode("/root/Global");
			
			global.Pseudo = pseudo;
			global.Volume = (float)volumeSlider.Value;
			global.Music = (float)musicSlider.Value;
			global.Sfx = (float)sfxSlider.Value;
			global.Fullscreen = fullScreenToggle.ButtonPressed;
			
			GD.Print($"Pseudo : {global.Pseudo}, Volume : {global.Volume}, Musique : {global.Music}, Effets : {global.Sfx}, Plein écran : {global.Fullscreen}");
		
			GD.Print("Lancement du jeu...");
			GetTree().ChangeSceneToFile("res://Scenes/main.tscn");
		}
		else
		{
			GD.Print("Veuillez entrez un pseudo valide");
		}
	}
	
	private void OnSettingsPressed()
	{
		GD.Print("Ouverture des paramètres...");
		settingsMenu.Visible = true;
		mainButtons.Visible = false;
		pseudoInput.Visible = false;
	}
	
	private void OnCreditsPressed()
	{
		GD.Print("Affichage des crédits...");
		creditsMenu.Visible = true;
		mainButtons.Visible = false;
		pseudoInput.Visible = false;
	}
	
	private void OnQuitPressed()
	{
		GD.Print("Quitter le jeu...");
		GetTree().Quit();
	}
	
	private void CloseSettings()
	{
		settingsMenu.Visible = false;
		mainButtons.Visible = true;
		pseudoInput.Visible = true;
	}

	private void CloseCredits()
	{
		creditsMenu.Visible = false;
		mainButtons.Visible = true;
		pseudoInput.Visible = true;
	}
	
	private void LoadHighScore()
	{
		highScoreBox.Visible = true;
		mainButtons.Visible = false;
		pseudoInput.Visible = false;
		
		try
		{
			string exeDir = Path.GetDirectoryName(OS.GetExecutablePath());
			string dbPath = ScoreDB.GetDBPath();
			
			if (!File.Exists(dbPath))
			{
				GD.PrintErr("Base de données introuvable : " + dbPath);
				return;
			}
			
			using var connection = new SQLiteConnection($"Data Source={dbPath}");
			connection.Open();
			
			using var command = connection.CreateCommand();
			command.CommandText = "SELECT Pseudo, Score FROM playerScore ORDER BY Score DESC LIMIT 5;";
			
			using SQLiteDataReader reader = command.ExecuteReader();
			
			StringBuilder highScoreText = new StringBuilder();
			int rank = 1;
			
			while (reader.Read())
			{
				string pseudo = reader.GetString(0);
				int score = reader.GetInt32(1);
				highScoreText.AppendLine($"{rank}. {pseudo} - {score} pts");
				rank++;
			}
			
			var scoreLabel = GetNode<Label>("CenterContainer/HighScoreBox/HighScoreList");
			if (scoreLabel != null)
			{
				GD.Print("Contenu highScore :\n" + highScoreText.ToString());
				scoreLabel.Text = highScoreText.ToString();
				scoreLabel.Visible = true;
			}
			else
			{
				GD.PrintErr("Impossible de trouver le noeud HighScoreList");
			}
		}
		catch (Exception ex)
		{
			GD.PrintErr("Erreur : " + ex.Message);
		}
	}
	
	private void CloseScores()
	{
		highScoreBox.Visible = false;
		mainButtons.Visible = true;
		pseudoInput.Visible = true;
	}
}
