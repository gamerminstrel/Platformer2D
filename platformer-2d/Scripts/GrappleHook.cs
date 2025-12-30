using Godot;
using System;

public partial class GrappleHook : Sprite2D
{
	private Area2D detectionRange;
	private PlayerCharacter playerInRange;

	private bool isGrappling = false;
	private Vector2 grappleTarget;
	private float grappleSpeed = 1200f; // Adjust for desired speed
	public override void _Ready()
	{
		detectionRange = GetNode<Area2D>("GrappleDetectionRange");
		detectionRange.BodyEntered += OnBodyEntered;
		detectionRange.BodyExited += OnBodyExited;
	}

	private void OnBodyEntered(Node body)
	{
		if (body is PlayerCharacter player)
		{
			playerInRange = player;
		}
	}

	private void OnBodyExited(Node body)
	{
		if (body == playerInRange)
		{
			playerInRange = null;
			Modulate = new Color(Colors.White.R, Colors.White.G, Colors.White.B, 0.5f); // Normal color (adjust as needed)
		}
	}

	public override void _Process(double delta)
	{
		if (isGrappling)
		{
			Vector2 currentPos = playerInRange.GlobalPosition;
			Vector2 direction = (grappleTarget - currentPos).Normalized();
			float step = grappleSpeed * (float)delta;
			float distance = currentPos.DistanceTo(grappleTarget);

			if (distance <= step)
			{
				playerInRange.GlobalPosition = grappleTarget;
				isGrappling = false;
				playerInRange.Velocity = new Vector2(playerInRange.Velocity.X, -250f); // Apply upward momentum

			}
			else
			{
				playerInRange.GlobalPosition += direction * step;
			}
		}
		if (playerInRange != null)
		{
			Vector2 toHook = GlobalPosition - playerInRange.GlobalPosition;
			bool facingHook = (playerInRange.Facing == PlayerCharacter.FacingDirection.Right && toHook.X > 0) ||
							  (playerInRange.Facing == PlayerCharacter.FacingDirection.Left && toHook.X < 0);

			// Raycast for line of sight
			var spaceState = GetWorld2D().DirectSpaceState;
			var excludeArray = new Godot.Collections.Array<Rid>
		{
			playerInRange.GetRid(),
            // this.GetRid()
        };
			var rayParams = new PhysicsRayQueryParameters2D
			{
				From = playerInRange.GlobalPosition,
				To = GlobalPosition,
				Exclude = excludeArray,
				// CollisionMask = 1 // Uncomment and adjust if you want to use a specific collision mask
			};
			var result = spaceState.IntersectRay(rayParams);

			bool obstructed = result.Count > 0;

			if (facingHook && !obstructed)
			{
				Modulate = Colors.White; // Solid white

				// Grapple action
				if (Input.IsActionJustPressed("grapple") && !isGrappling)
				{
					float offset = 50f;
					if (playerInRange.GlobalPosition.X < GlobalPosition.X)
						grappleTarget = GlobalPosition - new Vector2(offset, 0); // Left side
					else
						grappleTarget = GlobalPosition + new Vector2(offset, 0); // Right side

					isGrappling = true;
				}
			}
			else
			{
				Modulate = new Color(Colors.White.R, Colors.White.G, Colors.White.B, 0.5f); // Normal color
			}
		}
	}
}