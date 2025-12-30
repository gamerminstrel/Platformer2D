using Godot;
using System;

public partial class ResponsiveGrid : Node
{
	//public parameters and cutoff points
    [Export] public Godot.Collections.Array<Breakpoint> Breakpoints { get; set; } = new Godot.Collections.Array<Breakpoint>
    {
        new Breakpoint { Name = "sm", Width = 576 },
        new Breakpoint { Name = "md", Width = 768 },
        new Breakpoint { Name = "lg", Width = 992 },
        new Breakpoint { Name = "xl", Width = 1200 },
        new Breakpoint { Name = "xxl", Width = 1400 }
    };

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		/*

		*/
	}

	private void OnWindowResized()
    {
        GD.Print("Window was resized!");
		/*
		Flow:
		Get the new window size
		find all child elements that need to be repositioned/resized
		apply new positions/sizes based on some logic (e.g., percentage of window size)
		*/
		var newSize = GetViewport().GetVisibleRect().Size;

    }

}
