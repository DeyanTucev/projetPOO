using Godot;
using System;

public partial class Enemy : Area2D
{
    [Export] public PackedScene BulletScene;
    [Export] public float BulletSpeed = 200f;

    public override void _Ready()
    {
        GetTree().CreateTimer(2.0).Timeout += Shoot;
    }

    public void Shoot()
    {
        if (BulletScene == null)
        {
            GD.Print("BulletScene not assigned.");
            return;
        }

        var bullet = BulletScene.Instantiate<Node2D>();
        bullet.GlobalPosition = GlobalPosition;

        GetTree().Root.AddChild(bullet);
    }

    private void _on_Area2D_area_entered(Area2D area)
    {

        if (area.IsInGroup("PlayerBullet"))
        {
            QueueFree();
            area.QueueFree();
        }
    }
}
