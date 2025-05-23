using Godot;
using System;
using System.Collections.Generic;

public partial class EnemyContainer : Node2D
{
	[Export] public PackedScene EnemyScene;
	[Export] public int Columns = 8;
	[Export] public int Rows = 5;
	[Export] public float HorizontalSpacing = 100f;
	[Export] public float VerticalSpacing = 100f;
	[Export] public float MoveSpeed = 100f;
	[Export] public float MoveDownAmount = 50f;
	[Export] public float LeftBoundary = 100f;
	[Export] public float RightBoundary = 1100f;

	private Vector2 _direction = Vector2.Right;
	private float moveTimer = 0f;
	private float moveDelay = 1.0f;
	private bool shouldMoveDown = false;
	private Timer shootTimer;
	private Random random = new Random();
	private Vector2 initialPosition;

	public override void _Ready()
	{
		GD.Print("EnemyContainer ready, spawning enemies...");
		initialPosition = Position;
		SpawnEnemies();

		shootTimer = GetNode<Timer>("ShootTimer");
		shootTimer.Timeout += OnShootTimerTimeout;
		shootTimer.Autostart = true;
		shootTimer.Start();
	}

	public override void _Process(double delta)
	{
		moveTimer += (float)delta;
		if (moveTimer >= moveDelay)
		{
			if (shouldMoveDown)
			{
				Position += Vector2.Down * MoveDownAmount;
				shouldMoveDown = false;
			}
			else
			{
				Position += _direction * MoveSpeed;
				CheckBounds();
			}
			moveTimer = 0f;
		}
	}

	private void OnShootTimerTimeout()
	{
		var shooters = new List<Enemy>();
		foreach (Node child in GetChildren())
		{
			if (child is Enemy enemy)
			{
				shooters.Add(enemy);
			}
		}

		if (shooters.Count > 0)
		{
			int index = random.Next(shooters.Count);
			shooters[index].Shoot();
		}
	}

	private void CheckBounds()
	{
		foreach (Node child in GetChildren())
		{
			if (child is Enemy enemy)
			{
				float x = enemy.GlobalPosition.X;
				if (x > RightBoundary || x < LeftBoundary)
				{
					_direction = -_direction;
					shouldMoveDown = true;
					break;
				}
			}
		}
	}

	private void SpawnEnemies()
	{
		GD.Print("Spawning enemies...");
		float totalWidth = (Columns - 1) * HorizontalSpacing;
		float startX = -(totalWidth / 2f);

		for (int y = 0; y < Rows; y++)
		{
			for (int x = 0; x < Columns; x++)
			{
				var enemy = EnemyScene.Instantiate<Enemy>();
				float posX = startX + (x * HorizontalSpacing);
				float posY = y * VerticalSpacing;
				enemy.Position = new Vector2(posX, posY);
				AddChild(enemy);
			}
		}
	}

	public void OnEnemyKilled()
	{
		GD.Print("OnEnemyKilled called, checking enemy count...");

		// Lance un mini timer pour vérifier après la suppression effective
		var checkTimer = new Timer();
		checkTimer.OneShot = true;
		checkTimer.WaitTime = 0.1;
		AddChild(checkTimer);

		checkTimer.Timeout += () =>
		{
			int count = GetEnemyCount();
			GD.Print($"Enemies remaining: {count}");
			if (count == 0)
			{
				GD.Print("All enemies cleared, respawning...");
				var delayTimer = new Timer();
				delayTimer.OneShot = true;
				delayTimer.WaitTime = 1.0;
				AddChild(delayTimer);
				delayTimer.Timeout += () =>
				{
					Position = initialPosition;
					SpawnEnemies();
				};
				delayTimer.Start();
			}
		};
		checkTimer.Start();
	}

	private int GetEnemyCount()
	{
		int count = 0;
		foreach (var child in GetChildren())
		{
			if (child is Enemy)
			{
				count++;
			}
		}
		return count;
	}
}
