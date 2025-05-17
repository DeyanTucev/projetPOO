using Godot;
using System;
using System.Collections.Generic;

public partial class EnemyContainer : Node2D
{
    [Export] public PackedScene EnemyScene;
    [Export] public int Columns = 10;
    [Export] public int Rows = 5;
    [Export] public float HorizontalSpacing = 100f;
    [Export] public float VerticalSpacing = 100f;
    [Export] public float MoveSpeed = 40f;
    [Export] public float MoveDownAmount = 10f;

    private Vector2 _direction = Vector2.Right;
    private float moveTimer = 0f;
    private float moveDelay = 0.5f;

    public override void _Ready()
    {
        SpawnEnemies();
    }

    public override void _Process(double delta)
    {
        MoveEnemies((float)delta);
    }

    private void SpawnEnemies()
    {
        for (int y = 0; y < Rows; y++)
        {
            for (int x = 0; x < Columns; x++)
            {
                var enemy = EnemyScene.Instantiate<Area2D>();
                enemy.Position = new Vector2(x * HorizontalSpacing, y * VerticalSpacing);
                AddChild(enemy);
            }
        }
    }

    private void MoveEnemies(float delta)
    {
        Position += _direction * MoveSpeed * delta;

        foreach (Node child in GetChildren())
        {
            if (child is Area2D enemy)
            {
                Vector2 globalPos = enemy.GlobalPosition;
                if (globalPos.X > 700 || globalPos.X < 100)
                {
                    _direction = -_direction;
                    Position += Vector2.Down * MoveDownAmount;
                    break;
                }
            }
        }
    }
}
