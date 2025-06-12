using Godot;
using System;

public partial class Global : Node
{
	public string Pseudo { get; set; } = "";
	public int Score { get; set; } = 0;
	public float Volume { get; set; } = 1.0f;
	public float Music { get; set; } = 1.0f;
	public float Sfx { get; set; } = 1.0f;
	public bool Fullscreen { get; set; } = false;
}
