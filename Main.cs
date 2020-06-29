using Godot;

public class Main : Node2D
{
	[Export]
	PackedScene Mob;
	
    int score;

    private Timer _mobNode, _scoreNode, _startNode;

    private PathFollow2D _mobSpawnLocation;

	public override void _Ready()
	{
		GD.Randomize();

        _mobNode = GetNode<Timer>("MobTimer");
        _scoreNode = GetNode<Timer>("ScoreTimer");
        _startNode = GetNode<Timer>("StartTimer");

        _mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
    }

	public void GameOver()
	{
		_scoreNode.Stop();
		_mobNode.Stop();
	}

	public void Start()
	{
		score = 0;
		GetNode<Player>("Player").Start(
            GetNode<Position2D>("StartPosition").Position);
		_startNode.Start();
	}

    public void _on_MobTimer_timeout()
    {
        _mobSpawnLocation.Offset = GD.Randi();
        var mobInstance = (Enemy)Mob.Instance();
        AddChild(mobInstance);
        var direction = _mobSpawnLocation.Rotation + Mathf.Pi / 2;
        mobInstance.Position = _mobSpawnLocation.Position;
        direction += (float)GD.RandRange(-Mathf.Pi / 4f, Mathf.Pi / 4f);
        mobInstance.Rotation = direction;

        mobInstance.LinearVelocity = new Vector2((float)GD.RandRange(mobInstance.MinSpeed, mobInstance.MaxSpeed), 0).Rotated(direction);

    }

    public void _on_ScoreTimer_timeout()
    {
        score += 1;
    }


    public void _on_StartTimer_timeout()
    {
        _mobNode.Start();
        _scoreNode.Start();
    }
}



