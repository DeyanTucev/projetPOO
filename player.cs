using Godot;
using System;

public partial class player : Area2D
{
	[Export] public int Speed { get; set; } = 200;
	[Export] public float MinX { get; set; } = 0;
	[Export] public float MaxX { get; set; } = 1024;
	[Export] public float MinY { get; set; } = 0;
	[Export] public float MaxY { get; set; } = 600;
	[Export] public int MaxHp { get; set; } = 3;
	[Export] private int currentHp;
	
	[Signal] public delegate void HpChangedEventHandler(int currentHp);
	
	private Vector2 halfSize;

	public override void _Ready()
	{
		AddToGroup("Player");
		currentHp = MaxHp;
		
		// Get the CollisionShape2D and calculate half of the player's size
		var collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
		var shape = (RectangleShape2D)collisionShape.Shape;
		halfSize = shape.Size / 2.0f;

		// Connect the body_entered signal for collision detection
		Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
		
		EmitSignal(SignalName.HpChanged, currentHp);
	}
	
	private void OnBodyEntered(Node body)
	{
		GD.Print($"Touché par : {body.Name}");
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
	
	public override void _Process(double delta)
	{
		Vector2 velocity = Vector2.Zero;

		if (Input.IsKeyPressed(Key.A) || Input.IsKeyPressed(Key.Left))
			velocity.X -= 1;
		if (Input.IsKeyPressed(Key.D) || Input.IsKeyPressed(Key.Right))
			velocity.X += 1;

		// Normalize to ensure consistent speed diagonally
		if (velocity != Vector2.Zero)
			velocity = velocity.Normalized();

		Position += velocity * Speed * (float)delta;

		// Clamp player position inside the defined boundaries
		Vector2 min = new Vector2(MinX + halfSize.X, MinY + halfSize.Y);
		Vector2 max = new Vector2(MaxX - halfSize.X, MaxY - halfSize.Y);
		Position = Position.Clamp(min, max);
	}
}
