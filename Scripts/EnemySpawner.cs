using Godot;
using System;

public partial class EnemySpawner : Node
{
    private int waveCount = 0;
    private float speedMultiplier = 1.0f;
    private float shootIntervalMultiplier = 1.0f;

    [Export] public PackedScene EnemyScene;

    public void SpawnEnemies()
    {
        waveCount++;

        speedMultiplier = 1.0f + waveCount * 0.1f;
        shootIntervalMultiplier = 1.0f - waveCount * 0.05f;
        shootIntervalMultiplier = Mathf.Max(shootIntervalMultiplier, 0.3f); 

        for (int i = 0; i < 5; i++)
        {
            var enemy = EnemyScene.Instantiate() as Node2D;

            if (enemy is Enemy e)
            {
                e.SetSpeedMultiplier(speedMultiplier);
                e.SetShootRateMultiplier(shootIntervalMultiplier);
            }

            AddChild(enemy);
        }
    }
}