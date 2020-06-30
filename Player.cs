using Godot;
using System;

public class Player : Area2D
{
	[Signal]
	public delegate void Hit();

	[Export]
	public int Speed = 400;
	private Vector2 _screenSize;

    private string _colShapeLoc = "CollisionShape2D";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		_screenSize = GetViewportRect().Size;
		Hide();
	}

    public void Start(Vector2 position)
    {
        Position = position;
        Show();
        GetNode<CollisionShape2D>(_colShapeLoc).Disabled = false;
    }

    public override void _Process(float delta)
	{
		var velocity = new Vector2();

		if (Input.IsActionPressed("ui_right")) velocity.x += 1;
		if (Input.IsActionPressed("ui_left")) velocity.x -= 1;
		if (Input.IsActionPressed("ui_up")) velocity.y -= 1;
		if (Input.IsActionPressed("ui_down")) velocity.y += 1;

		var animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");

		if (velocity.Length() > 0)
		{
			velocity = velocity.Normalized() * Speed;
			animatedSprite.Play();
		}
		else animatedSprite.Stop();

		Position += velocity * delta;
		Position = new Vector2(
			x: Mathf.Clamp(Position.x, 0, _screenSize.x),
			y: Mathf.Clamp(Position.y, 0, _screenSize.y)
		);

        if (Math.Abs(velocity.x) > Mathf.Epsilon)
        {
            animatedSprite.Animation = "walk";
            animatedSprite.FlipV = false;

            animatedSprite.FlipH = velocity.x < 0;
        }

        else if (Math.Abs(velocity.y) > Mathf.Epsilon) 
		{
			animatedSprite.Animation = "up";
			animatedSprite.FlipV = velocity.y > 0;
		}
	}

    public void OnPlayerBodyEntered(PhysicsBody2D body)
    {
        Hide();
        EmitSignal("Hit");
        GetNode<CollisionShape2D>(_colShapeLoc).SetDeferred("disabled", true);
    }
}
