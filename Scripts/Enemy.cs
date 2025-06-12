using Godot;
using System;

public partial class Enemy : Area2D
{
	[Export] public PackedScene BulletScene;
	[Export] public float BulletSpeed = 200f; // Speed of the bullet

	private Timer shootTimer;

	public override void _Ready()
	{
		base._Ready();
		var collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
		var shape = (RectangleShape2D)collisionShape.Shape;
		if(!IsConnected("area_entered", new Callable(this, nameof(_on_Area2D_area_entered))))
		{
			Connect("area_entered", new Callable(this, nameof(_on_Area2D_area_entered)));
		}
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
		if (!IsInsideTree() || !Visible)
		{
			return;
		}
		
		GD.Print("===> Area entered: ", area.Name);

		// Affiche tous les groupes de ce node
		foreach (string group in area.GetGroups())
		{
			GD.Print("Group: ", group);
		}
		if (area.IsInGroup("PlayerBullet"))
		{
			GD.Print("===> PlayerBullet detected! (traitement en cours)");
			area.QueueFree();
			var playerNode = GetTree().GetFirstNodeInGroup("Player") as player;
			if (playerNode != null)
			{
				playerNode.AddScore(5);
			}
			
			GetNode<CollisionShape2D>("CollisionShape2D").CallDeferred("set_disabled", true);

			// Vérifie si le container existe
			if (GetParent() is EnemyContainer container)
			{
				container.OnEnemyKilled();
			}
			QueueFree();
		}
	}
}
