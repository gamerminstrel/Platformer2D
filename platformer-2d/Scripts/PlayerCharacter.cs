using Godot;
using System;

public partial class PlayerCharacter : CharacterBody2D
{
	// Gravity and terminal velocity values (adjust as needed)
	[Export] public float Gravity = 1200f;
	[Export] public float TerminalVelocity = 2000f;
	[Export] public float MoveSpeed = 300f;
	[Export] public float JumpVelocity = 600f;



	public bool IsOnGround { get; private set; } = false;

	public enum FacingDirection { Left, Right }
	public FacingDirection Facing { get; private set; } = FacingDirection.Right;


	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Handle left/right input
		float input = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
		velocity.X = input * MoveSpeed;

		if (input < 0)
			Facing = FacingDirection.Left;
		else if (input > 0)
			Facing = FacingDirection.Right;

		// Check if on ground using built-in method
		IsOnGround = IsOnFloor();

		// Jumping
		if (IsOnGround && Input.IsActionJustPressed("ui_accept"))
		{
			velocity.Y = -JumpVelocity;
		}
		else if (!IsOnGround)
		{
			// Apply gravity
			velocity.Y += Gravity * (float)delta;
			if (velocity.Y > TerminalVelocity)
				velocity.Y = TerminalVelocity;
		}
		else
		{
			// Optionally reset vertical velocity when on ground
			velocity.Y = 0;
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
