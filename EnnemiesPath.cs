using Godot;
using System;

public partial class EnnemiesPath : PathFollow2D
{
    [Export]
    public float Speed = 100f;
    private float progress = 0f;

    public override void _Ready()
    {
        // Called when the node is added to the scene for the first time.
        // Initialize any variables or state here.
    }

    public override void _Process(double delta)
    {
        // Called every frame. 'delta' is the elapsed time since the previous frame.
        // Update your game logic here.
        
        progress += Speed * (float)delta;
        Progress = progress;
    }
}
