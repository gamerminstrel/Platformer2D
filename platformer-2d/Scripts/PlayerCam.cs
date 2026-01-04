using Godot;
using System;

public partial class PlayerCam : Camera2D
{
	// Called when the node enters the scene tree for the first time.
public override void _Ready()
{
	// Adjust zoom based on viewport size to maintain consistent view
    var viewportSize = GetViewport().GetVisibleRect().Size;
    Zoom = new Vector2(viewportSize.X / 640f, viewportSize.Y / 360f);
}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
