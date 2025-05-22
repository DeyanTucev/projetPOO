using Godot;
using System;

public partial class HpLabel : Node2D
{
	private Label label;
	private Timer retryTimer;

	public override void _Ready()
	{
		label = GetNode<Label>("HpLabel");
		if (label == null)
		{
			GD.PrintErr("Label introuvable !");
			return;
		}
		GD.Print("Label node trouvé : HpLabel");
		GD.Print($"Label texte initial : '{label.Text}'");

		retryTimer = GetNode<Timer>("Timer");
		retryTimer.OneShot = true;
		retryTimer.Timeout += TryConnectToPlayer;

		TryConnectToPlayer();
	}

	private void TryConnectToPlayer()
	{
		var players = GetTree().GetNodesInGroup("Player");
		if (players.Count == 0)
		{
			GD.Print("Pas de player encore, réessaie dans 0.5s");
			retryTimer.Start(0.5f);
			return;
		}

		var player = players[0];
		player.Connect("HpChanged", new Callable(this, nameof(OnHpChanged)));
		GD.Print("Connexion au signal HpChanged réussie !");
	}

	private void OnHpChanged(int hp)
	{
		GD.Print($"Signal HpChanged reçu avec hp={hp}");
		label.Text = $"HP : {hp}";
	}
}
