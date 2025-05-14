using Godot;
using System;

public partial class player : Area2D
{
    [Export]
    public int Speed { get; set; } = 10;

    public Vector2 ScreenSize;

    public override void _Ready()
    {
        ScreenSize = GetViewport().GetVisibleRect().Size;
    }

    public override void _Process(double delta)
    {

        var velocity = Vector2.Zero;
        Position += velocity * (float)delta;
        Position = new Vector2(
            x: Mathf.Clamp(Position.X, 0, ScreenSize.X),
            y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y)
        );

        if (Input.IsKeyPressed(Key.Right))
        {
            velocity.X += Speed;
        }
        if (Input.IsKeyPressed(Key.Left))
        {
            velocity.X -= Speed;
        }
        
    }
}
