using Godot;

public class Enemy : RigidBody2D
{
	[Export]
	public int MinSpeed = 150;

	[Export]
	public int MaxSpeed = 250;

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
