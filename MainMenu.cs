using Godot;
using System;

public partial class MainMenu : Control
{
	private Control settingsMenu;
	private Control creditsMenu;
	private Control mainButtons;
	
	public override void _Ready()
	{
		settingsMenu = GetNode<Control>("CenterContainer/SettingsMenu");
		creditsMenu = GetNode<Control>("CenterContainer/CreditsMenu");
		mainButtons = GetNode<Control>("CenterContainer/MainButtons");
		
		GetNode<Button>("CenterContainer/SettingsMenu/back").Pressed += CloseSettings;
		GetNode<Button>("CenterContainer/CreditsMenu/back").Pressed += CloseCredits;
		
		settingsMenu.Visible = false;
		creditsMenu.Visible = false;
		
		GetNode<Button>("CenterContainer/MainButtons/play").Pressed += OnPlayPressed;
		GetNode<Button>("CenterContainer/MainButtons/settings").Pressed += OnSettingsPressed;
		GetNode<Button>("CenterContainer/MainButtons/credits").Pressed += OnCreditsPressed;
		GetNode<Button>("CenterContainer/MainButtons/quit").Pressed += OnQuitPressed;
	}
	
	private void OnPlayPressed()
	{
		GD.Print("Lancement du jeu...");
		GetTree().ChangeSceneToFile("res://player.tscn");
	}
	
	private void OnSettingsPressed()
	{
		GD.Print("Ouverture des paramètres...");
		settingsMenu.Visible = true;
		creditsMenu.Visible = false;
		mainButtons.Visible = false;
	}
	
	private void OnCreditsPressed()
	{
		GD.Print("Affichage des crédits...");
		settingsMenu.Visible = false;
		creditsMenu.Visible = true;
		mainButtons.Visible = false;
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
	}

	private void CloseCredits()
	{
		creditsMenu.Visible = false;
		mainButtons.Visible = true;
	}
}
