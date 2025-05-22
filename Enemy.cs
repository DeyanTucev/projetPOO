using Godot;
using System;

public partial class Enemy : Area2D
{
    [Export] public PackedScene BulletScene;
    [Export] public float BulletSpeed = 200f; // Speed of the bullet
    [Export] public bool CanShoot = false; // Flag to control shooting

    private Timer shootTimer;

    public override void _Ready()
    {
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
        var bulletNode = BulletScene.Instantiate();
        if (bulletNode is EnemyBullet bullet)
        {

            bullet.GlobalPosition = GlobalPosition;
            
            GetTree().Root.AddChild(bullet);
        }
        else
        {
            GD.PrintErr("BulletScene is not an EnemyBullet!");
        }
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
