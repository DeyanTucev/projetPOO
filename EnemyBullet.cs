using Godot;
using System;

public partial class EnemyBullet : Area2D
{
    [Export] public float Speed = 200f; // Speed of the bullet

    public override void _Process(double delta)
    {
        Position += Vector2.Down * Speed * (float)delta;

        if (Position.Y > GetViewport().GetVisibleRect().Size.Y)
        {
            QueueFree(); // Remove the bullet if it goes off-screen
        }
    }

    private void _on_Area2D_area_entered(Area2D area)
    {
        if (area.IsInGroup("Player"))
        {
            QueueFree();
        }
    }

}
