using Godot;
using System;

public partial class Enemy : Area2D
{
    [Export] public PackedScene BulletScene;
    [Export] public float BulletSpeed = 200f; // Speed of the bullet

    public override void _Ready()
    {
        GetTree().CreateTimer(2.0).Timeout += Shoot; // Call Shoot every 2 seconds
    }

    public void Shoot()
    {
        // Check if BulletScene is assigned
        // and if the bullet can be instantiated
        if (BulletScene == null)
        {
            GD.Print("BulletScene not assigned.");
            return;
        }

        var bullet = BulletScene.Instantiate<Node2D>();
        bullet.GlobalPosition = GlobalPosition;

        GetTree().Root.AddChild(bullet); // Add the bullet to the scene tree
    }

    private void _on_Area2D_area_entered(Area2D area)
    {

        // Check if the area is in the "Player" group
        if (area.IsInGroup("PlayerBullet"))
        {
            QueueFree();
            area.QueueFree();
        }
    }
}
