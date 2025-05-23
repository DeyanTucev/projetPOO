using Godot;
using System;

public partial class player : Area2D
{
	[Export] public PackedScene PlayerShootScene;
	[Export] public float Speed { get; set; } = 1000f;
	[Export] public float MinX { get; set; } = 0f;
	[Export] public float MaxX { get; set; } = 1200f;
	[Export] public int MaxHp { get; set; } = 3;
	[Export] private int currentHp;
	
	[Signal] public delegate void HpChangedEventHandler(int currentHp);

	private Timer ShootTimer;
	public Vector2 ScreenSize;
	private Vector2 halfSize;

	public override void _Ready()
	{
		AddToGroup("Player");
		currentHp = MaxHp;
		
		ShootTimer = GetNode<Timer>("ShootTimer");
		ScreenSize = GetViewportRect().Size;

		var collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
		var shape = (CapsuleShape2D)collisionShape.Shape;

		// Connexion du signal de collision
		base._Ready();
		Connect("area_entered", new Callable(this, nameof(OnAreaEntered)));
	}

	public override void _Process(double delta)
	{
		Vector2 velocity = Vector2.Zero;

		if (Input.IsKeyPressed(Key.A) || Input.IsKeyPressed(Key.Left))
			velocity.X -= 1;
		if (Input.IsKeyPressed(Key.D) || Input.IsKeyPressed(Key.Right))
			velocity.X += 1;

		Position += velocity.Normalized() * Speed * (float)delta;

		Vector2 min = new Vector2(MinX - halfSize.X, Position.Y);
		Vector2 max = new Vector2(MaxX - halfSize.X, Position.Y);
		Position = Position.Clamp(min, max);

		// Player shooting
		if (Input.IsKeyPressed(Key.Space) && !ShootTimer.IsStopped())
		return;
		  
		if (Input.IsKeyPressed(Key.Space) && ShootTimer.IsStopped())
		{
			Shoot();
			ShootTimer.Start();
		}
	} 

	public void Shoot()
	{
		// Check if BulletScene is assigned
		// and if the bullet can be instantiated
		if (PlayerShootScene == null)
		{
			GD.Print("PlayerShootScene not assigned.");
			return;
		}
		var bulletNode = PlayerShootScene.Instantiate();
		if (bulletNode is Area2D bullet)
		{ 
			bullet.GlobalPosition = GlobalPosition;
			GetTree().Root.AddChild(bullet);
		}
		else
		{
			GD.PrintErr("BulletScene is not a PlayerBullet!");
		}
	}

	private void OnAreaEntered(Area2D area)
	{
		GD.Print("Area entered: " + area.Name);
		TakeDamage(1);
	}
	
	private void TakeDamage(int amount)
	{
		currentHp -= amount;
		currentHp = Mathf.Max(currentHp, 0);
		
		EmitSignal(SignalName.HpChanged, currentHp);
		
		if (currentHp == 0)
		{
			Die();
		}
		else
		{
			GD.Print($"HP : {currentHp}");
		}
	}
	
	private void Die()
	{
		GD.Print("Le joueur est mort !");
		QueueFree();
	}
}
