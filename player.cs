using Godot;
using System;

public partial class player : Area2D
{
    [Export] public float Speed { get; set; } = 1000f;
    [Export] public float MinX { get; set; } = 0f;
    [Export] public float MaxX { get; set; } = 1200f;
    [Export] public float MinY { get; set; } = 0f;
    [Export] public float MaxY { get; set; } = 0f;


    public Vector2 ScreenSize;
    private Vector2 halfSize;

    public override void _Ready()
    {
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

        Vector2 min = new Vector2(MinX - halfSize.X, MinY - halfSize.Y);
        Vector2 max = new Vector2(MaxX - halfSize.X, MaxY - halfSize.Y);
        Position = Position.Clamp(min, max);
    } 

    private void OnAreaEntered(Area2D area)
    {
        GD.Print("Area entered: " + area.Name);
        QueueFree();
    }
}
