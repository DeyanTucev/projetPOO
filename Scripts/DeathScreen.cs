using Godot;
using System;

public partial class DeathScreen : Control
{
	private Control DeathButtons;
	
	public override void _Ready()
	{
		DeathButtons = GetNode<Control>("CenterContainer/DeathButtons");
		
		GetNode<Button>("CenterContainer/DeathButtons/Replay").Pressed += OnPlayPressed;
		GetNode<Button>("CenterContainer/DeathButtons/back").Pressed += OnBackPressed;
	}
	
	private void OnPlayPressed()
	{
		GD.Print("Lancement du jeu...");
		GetTree().ChangeSceneToFile("res://Scenes/main.tscn");
	}
	
	private void OnBackPressed()
	{
		GD.Print("Retour au menu...");
		GetTree().ChangeSceneToFile("res://Scenes/main_menu.tscn");
	}
}
