using Godot;
using System;

public class Main : Node2D
{
	[Export]
	PackedScene Mob;
	
	int score;

	private Timer _mobNode, _scoreNode, _startNode;

	private PathFollow2D _mobSpawnLocation;

	private HUD _HUDNode;

	private Random _random = new Random();

	private float RandRange(float min, float max)
	{
		return (float)_random.NextDouble() * (max - min) + min;
	}

	public override void _Ready()
	{
		_mobNode = GetNode<Timer>("MobTimer");
		_scoreNode = GetNode<Timer>("ScoreTimer");
		_startNode = GetNode<Timer>("StartTimer");

		_HUDNode = GetNode<HUD>("HUD");

		_mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");

		NewGame();
	}

	public void GameOver()
	{
		_scoreNode.Stop();
		_mobNode.Stop();

		_HUDNode.ShowGameOver();
		GetTree().CallGroup("mobs", "queue_free");
	}

	public void NewGame()
	{
		score = 0;
		GetNode<Player>("Player").Start(
			GetNode<Position2D>("StartPosition").Position);
		_startNode.Start();

		_HUDNode.UpdateScore(score);
		_HUDNode.ShowMessage("Get ready!");
	}

	public void _on_MobTimer_timeout()
	{
		_mobSpawnLocation.Offset = _random.Next();
		var mobInstance = (Enemy)Mob.Instance();
		AddChild(mobInstance);
		var direction = _mobSpawnLocation.Rotation + Mathf.Pi / 2;
		mobInstance.Position = _mobSpawnLocation.Position;
		direction += (float)GD.RandRange(-Mathf.Pi / 4f, Mathf.Pi / 4f);
		mobInstance.Rotation = direction;

		mobInstance.LinearVelocity =
			new Vector2(RandRange(mobInstance.MinSpeed, mobInstance.MaxSpeed), 0).Rotated(direction);
	}

	public void _on_ScoreTimer_timeout()
	{
		score += 1;

		_HUDNode.UpdateScore(score);
	}


	public void _on_StartTimer_timeout()
	{
		_mobNode.Start();
		_scoreNode.Start();
	}
}
