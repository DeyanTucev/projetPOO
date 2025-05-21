using Godot;
using System;

public partial class EnemyBullet : Node2D
{
    public Vector2 Direction = Vector2.Down;
    public float Speed = 200f;

    public override void _Process(double delta)
    {
        Position += Direction * Speed * (float)delta;
    }

    private void _on_Area2D_area_entered(Area2D area)
    {
        if (area.IsInGroup("Player"))
        {
            QueueFree();
        }
    }

}
