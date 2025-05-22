using Godot;
using System;

public partial class player : Area2D
{
    [Export]
    public int Speed { get; set; } = 200;
    [Export] public float MinX { get; set; }
    [Export] public float MaxX { get; set; }
    [Export] public float MinY { get; set; }
    [Export] public float MaxY { get; set; }

    private Vector2 screenSize;
    private Vector2 halfSize;

    public override void _Ready()
    {
        screenSize = GetViewportRect().Size;

        var collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        var shape = (RectangleShape2D)collisionShape.Shape;
        halfSize = shape.Size / 2.0f;

        // Connexion du signal de collision
        Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
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

    private void OnBodyEntered(Node body)
    {
        GD.Print($"Collision détectée avec : {body.Name}");
        QueueFree();


    }
}
