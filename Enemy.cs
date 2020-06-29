using Godot;

public class Enemy : RigidBody2D
{
	[Export]
	private int _minSpeed = 150;

	[Export]
	private int _maxSpeed = 250;

	private string[] _mobTypes = { "walk", "swim", "fly" };

	public override void _Ready()
	{
		GetNode<AnimatedSprite>("AnimatedSprite").Animation =
			_mobTypes[GD.Randi() % _mobTypes.Length];

	}

    private void _on_Visibility_screen_exited()
    {
        QueueFree();
    }
}
