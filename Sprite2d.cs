using Godot;
using System;

public partial class Sprite2d : Sprite2D
{
	public override void _Ready()
	{
		GD.Print("Sprite2d is ready");
	}

   /* public override void _Process(double delta)
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

	}*/
}
