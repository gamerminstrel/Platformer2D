using Godot;
using System;

public partial class UiIndicator : AnimatedSprite2D
{
    private Vector2 targetPosition;
    private bool isSliding = false;
    private Button currentButton = null;

	private float idleTime = 0f;
	private Vector2 idleOffset = Vector2.Zero;

    public override void _Ready()
    {
        targetPosition = GlobalPosition;
    }

	public override void _Process(double delta)
	{
		var focusedButton = FindFocusedButton();
		if (focusedButton != null && focusedButton != currentButton)
		{
			currentButton = focusedButton;
			SetTargetAboveButton(currentButton);
			isSliding = true;
		}

		if (isSliding)
		{
			// Smoothly slide to the target position (no idle offset while sliding)
			GlobalPosition = GlobalPosition.Lerp(targetPosition, (float)(delta * 10.0));
			if (GlobalPosition.DistanceTo(targetPosition) < 1.0f)
			{
				GlobalPosition = targetPosition;
				isSliding = false;
				idleTime = 0f; // Reset idle animation phase for smoothness
			}
		}
		else
		{
			ApplyIdleAnimation(delta);
		}
	}

	private void ApplyIdleAnimation(double delta)
	{
		idleTime += (float)delta;

		// Idle breathing animation (sinusoidal up/down, slight side-to-side)
		float y = Mathf.Sin(idleTime * 2.0f) * 15f; // Up/down, max 15px
		float x = Mathf.Sin(idleTime * 1.3f + Mathf.Cos(idleTime * 0.7f) * 2f) * 7f; // Side-to-side, max ~7px
		idleOffset = new Vector2(x, y);

		// Clamp total movement to 10px from target
		var finalPos = targetPosition + idleOffset;
		if (finalPos.DistanceTo(targetPosition) > 10f)
		{
			finalPos = targetPosition + (idleOffset.Normalized() * 10f);
		}
		GlobalPosition = finalPos;
	}

	private Button FindFocusedButton()
	{
		var panel = GetParent();
		if (panel == null)
			return null;

		return FindFocusedButtonRecursive(panel);
	}

	private Button FindFocusedButtonRecursive(Node node)
	{
		foreach (Node child in node.GetChildren())
		{
			if (child is Button button && button.HasFocus())
				return button;

			var found = FindFocusedButtonRecursive(child);
			if (found != null)
				return found;
		}
		return null;
	}


	private void SetTargetAboveButton(Button button)
	{
		var globalRect = button.GetGlobalRect();
		var buttonRectPos = globalRect.Position;
		var buttonRectSize = globalRect.Size;

		// Get the current frame's texture size
		var frameTexture = SpriteFrames?.GetFrameTexture(Animation, Frame);
		var textureSize = frameTexture != null ? frameTexture.GetSize() : Vector2.Zero;

		// Center the indicator horizontally above the button, 100px above
		var buttonCenterX = buttonRectPos.X + buttonRectSize.X / 2.0f;
		var indicatorX = buttonCenterX - textureSize.X / 2.0f;
		var indicatorY = buttonRectPos.Y - 15 - textureSize.Y;

		targetPosition = new Vector2(indicatorX, indicatorY);
	}
}