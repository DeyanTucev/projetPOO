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
    [Export] public float LeftBoundary = 100f; // Left boundary for enemy movement
    [Export] public float RightBoundary = 1100f; // Right boundary for enemy movement

    private Vector2 _direction = Vector2.Right;
    private float moveTimer = 0f; // Timer to control movement delay
    private float moveDelay = 1.0f; // Delay between movements
    private bool shouldMoveDown = false;

    public override void _Ready()
    {
        SpawnEnemies();
    }

    public override void _Process(double delta)
    {
        moveTimer += (float)delta;
        if (moveTimer >= moveDelay) // Check if the delay has passed
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
            };
            moveTimer = 0f;
        }
    }

    private void CheckBounds() 
    {
        foreach (Node child in GetChildren()) // Check each child node
        {
            if (child is Area2D enemy) // Check if the child is an Area2D (enemy)
            
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

    private void SpawnEnemies() // Spawns enemies in a grid formation
    {
        float totalWidth = (Columns - 1) * HorizontalSpacing;
        float startX = -(totalWidth / 2f);

        for (int y = 0; y < Rows; y++)
        {
            for (int x = 0; x < Columns; x++)
            {
                var enemy = EnemyScene.Instantiate<Area2D>(); // Instantiate the enemy scene
                float posX = startX + (x * HorizontalSpacing);
                float posY = y * VerticalSpacing;
                enemy.Position = new Vector2(posX, posY); // Set the position of the enemy
                AddChild(enemy); // Add the enemy to the container
            }
        }
    }
}
