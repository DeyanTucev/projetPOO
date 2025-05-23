using Godot;
using System;

public partial class PlayerShootScene : Area2D
{
	[Export] public float Speed = 600f; // Speed of the bullet

	public override void _Process(double delta)
	{
		Position += Vector2.Up * Speed * (float)delta;

		if (Position.Y > GetViewport().GetVisibleRect().Size.Y)
		{
			QueueFree(); // Remove the bullet if it goes off-screen
		}
	}

	private void _on_Area2D_area_entered(Area2D area)
	{
		if (area.IsInGroup("Enemy"))
		{
			area.QueueFree(); // Remove the enemy
			QueueFree();
		}
	}

}
