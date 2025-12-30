using Godot;
using System;

public partial class SimpleOverlay : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		DisplayHelper.ResizeToWindow(this);

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	// private void ResizeToWindow()
	// {
	// 	AnchorLeft = 0;
	// 	AnchorTop = 0;
	// 	AnchorRight = 1;
	// 	AnchorBottom = 1;
	// 	OffsetLeft = 0;
	// 	OffsetTop = 0;
	// 	OffsetRight = 0;
	// 	OffsetBottom = 0;
	// 	SizeFlagsHorizontal = SizeFlags.Expand;
	// 	SizeFlagsVertical = SizeFlags.Expand;
	// }
}
