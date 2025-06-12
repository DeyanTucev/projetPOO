using Godot;
using System;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

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
	private int score = 0;
	private bool tookDMG = false;
	private Global globalPseudo;
	private string pseudo = "";
	private AudioStreamPlayer2D shootSound;

	private async Task BlinkEffect()
	{
		tookDMG = true;
		float totalTime = 1.0f;
		float blinkInterval = 0.1f;
		float elapsed = 0f;

		while (elapsed < totalTime)
		{
			Visible = !Visible;
			await ToSignal(GetTree().CreateTimer(blinkInterval), "timeout");
			elapsed += blinkInterval;
		}

		Visible = true;
		tookDMG = false;
	}


	public override void _Ready()
	{
		AddToGroup("Player");
		currentHp = MaxHp;
		
		ShootTimer = GetNode<Timer>("ShootTimer");
		ScreenSize = GetViewportRect().Size;
		
		var collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
		var shape = (CapsuleShape2D)collisionShape.Shape;
		shootSound = GetNode<AudioStreamPlayer2D>("ShootSound");
		// Connexion du signal de collision
		base._Ready();
		Connect("area_entered", new Callable(this, nameof(OnAreaEntered)));
	}
	public void AddScore(int amount)
	{
		var ScoreLabel = GetNode<Label>("../Node2D/ScoreLabel");
		score += amount;
		if (ScoreLabel != null)
			ScoreLabel.Text = $"Score: {score}";
		else
			GD.PrintErr("ScoreLabel is not assigned!");
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
    if (PlayerShootScene == null)
    {
        GD.Print("PlayerShootScene not assigned.");
        return;
    }

    // Crée un nouveau AudioStreamPlayer2D pour ne pas couper les sons précédents
    var shootSoundInstance = new AudioStreamPlayer2D();
    shootSoundInstance.Stream = shootSound.Stream; // reprend le même son que le node existant
    AddChild(shootSoundInstance);
    shootSoundInstance.Play();

    // Supprimer le son après la durée du stream
    float duration = (float)shootSoundInstance.Stream.GetLength();
    var timer = new Timer();
    timer.WaitTime = duration;
    timer.OneShot = true;
    AddChild(timer);
    timer.Start();

    timer.Timeout += () =>
    {
        shootSoundInstance.QueueFree();
        timer.QueueFree();
    };

    // Instancier la balle
    var bulletNode = PlayerShootScene.Instantiate();
    if (bulletNode is Area2D bullet)
    {
        bullet.GlobalPosition = GlobalPosition;
        bullet.AddToGroup("PlayerBullet");
        GetTree().Root.AddChild(bullet);
    }
    else
    {
        GD.PrintErr("BulletScene is not a PlayerBullet!");
    }}

	private void OnAreaEntered(Area2D area)
	{
		GD.Print("Zone entrée: " + area.Name);
		
		if (area.IsInGroup("EnemyBullet") || area.IsInGroup("Enemy"))
		{
			area.QueueFree();
			
			if (area.IsInGroup("EnemyBullet"))
			{
				GD.Print("Collision balle ennemi.");
				area.QueueFree();
			}

			if (area.IsInGroup("Enemy"))
			{
				GD.Print("Collision ennemi.");
				Die();
				return;
			}
			
			TakeDamage(1);
		}
	}
	
	private async void TakeDamage(int amount)
	{
		if (tookDMG){
			return;
		}
			
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
			await BlinkEffect();
		}
	}
	
	private void Die()
	{
		GD.Print("Le joueur est mort !");
		
		var enemyBullets = GetTree().GetNodesInGroup("EnemyBullet");
		foreach (Node bullet in enemyBullets)
		{
			bullet.QueueFree();
		}
		
		var playerBullets = GetTree().GetNodesInGroup("PlayerBullet");
		foreach (Node bullet in playerBullets)
		{
			bullet.QueueFree();
		}
		
		Global global = (Global)GetNode("/root/Global");
		string pseudo = global.Pseudo;
		
		GD.Print(pseudo);
		
		try
		{
			string exeDir = Path.GetDirectoryName(OS.GetExecutablePath());
			string dbPath = ScoreDB.GetDBPath();
			
			using var connection = new SQLiteConnection($"Data Source={dbPath}");
			connection.Open();
			
			using var insertCommand = connection.CreateCommand();
			insertCommand.CommandText = @"INSERT INTO playerScore (Pseudo, Score) VALUES (@pseudo, @score);";
			insertCommand.Parameters.AddWithValue("@pseudo", pseudo);
			insertCommand.Parameters.AddWithValue("@score", score);
			insertCommand.ExecuteNonQuery();
			
			GD.Print("Score sauvgardé !");
		}
		catch (Exception ex)
		{
			GD.PrintErr("Erreur : " + ex.Message);
		}
		QueueFree();
		
		GetTree().ChangeSceneToFile("res://Scenes/DeathScreen.tscn");
	}
}
