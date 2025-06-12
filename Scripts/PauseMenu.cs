using Godot;
using System;

public partial class PauseMenu : Node2D
{
	private Button pauseButton;
	private Timer retryTimer;
	private Control PauseBox;
	
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
		GetTree().Paused = false;
		GetTree().ChangeSceneToFile("res://Scenes/main_menu.tscn");
	}
}
