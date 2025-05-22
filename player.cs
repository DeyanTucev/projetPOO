using Godot;
using System;

public partial class player : Area2D
{
	[Export]
	public int Speed { get; set; } = 10;

	public Vector2 ScreenSize;

	public override void _Ready()
	{
		ScreenSize = GetViewport().GetVisibleRect().Size;
	}

	public override void _Process(double delta)
	{

		int movement = 10;
		if (Input.IsKeyPressed(Key.A) || Input.IsKeyPressed(Key.Left))
		{
			Position += new Vector2(-movement, 0);
		}
		if (Input.IsKeyPressed(Key.D) || Input.IsKeyPressed(Key.Right))
		{
			Position += new Vector2(movement, 0);
		}
		
	}
}
