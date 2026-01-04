using Godot;
using System;

namespace Platformer2D.Scripts
{
    // BUG FIX: This class should not be PlayerCharacter, and should not duplicate PlayerCharacter's code.
    // Instead, this should be a GrappleHook class.
    // Remove all PlayerCharacter code and implement GrappleHook logic.

    public partial class GrappleHook : Sprite2D
    {
        private Area2D detectionRange;
        private PlayerCharacter playerInRange;

        private bool isGrappling = false;
        private Vector2 grappleTarget;
        [Export]
        private float grappleSpeed = 3500f; // Adjust for desired speed

        // Cache the ShaderMaterial for toggling outline
        private ShaderMaterial outlineMaterial;

        public override void _Ready()
        {
            detectionRange = GetNode<Area2D>("GrappleDetectionRange");
            detectionRange.BodyEntered += OnBodyEntered;
            detectionRange.BodyExited += OnBodyExited;

            // Get the assigned ShaderMaterial (assumes it's set in the editor)
            outlineMaterial = Material as ShaderMaterial;
            if (outlineMaterial is not null)
            {
                outlineMaterial.SetShaderParameter("outline_enabled", false);
            }
        }

        private void SetOutlineEnabled(bool enabled)
        {
            if (outlineMaterial is not null)
            {
                outlineMaterial.SetShaderParameter("outline_enabled", enabled);
            }
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
                SetOutlineEnabled(false);
            }
        }

        public override void _Process(double delta)
        {
            if (isGrappling && playerInRange is not null)
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
            if (playerInRange is not null)
            {
                Vector2 toHook = GlobalPosition - playerInRange.GlobalPosition;
                bool facingHook = (playerInRange.Facing == PlayerCharacter.FacingDirection.Right && toHook.X > 0) ||
                                  (playerInRange.Facing == PlayerCharacter.FacingDirection.Left && toHook.X < 0);

                // Raycast for line of sight
                var spaceState = GetWorld2D().DirectSpaceState;
                var excludeArray = new Godot.Collections.Array<Rid>
                {
                    playerInRange.GetRid(),
                };
                var rayParams = new PhysicsRayQueryParameters2D
                {
                    From = playerInRange.GlobalPosition,
                    To = GlobalPosition,
                    Exclude = excludeArray,
                };
                var result = spaceState.IntersectRay(rayParams);

                bool obstructed = result.Count > 0;

                if (facingHook && !obstructed)
                {
                    Modulate = Colors.White; // Solid white
                    SetOutlineEnabled(true);

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
                    SetOutlineEnabled(false);
                }
            }
            else
            {
                SetOutlineEnabled(false);
            }
        }
    }
}