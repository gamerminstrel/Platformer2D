using Godot;
// ...existing code...
public partial class DevControls : Node2D
{
	//Set up variables
	private float escHoldTime = 0f;
	private bool escHeld = false;
	private Node overlayInstance = null;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetProcessInput(true);
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey keyEvent
			  && keyEvent.Pressed
			  && keyEvent.Keycode == Key.Escape
			  && !keyEvent.Echo) // Only trigger on initial press, not repeats
		{
			ToggleDimOverlay();
		}
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		CheckEscHoldToQuit(delta);

	}

	/// <summary>
	/// Checks if the Escape key is held for 3 seconds to quit the application.
	/// </summary>
	/// <param name="delta"></param>
	private void CheckEscHoldToQuit(double delta)
	{
		if (Input.IsKeyPressed(Key.Escape))
		{
			if (!escHeld)
			{
				escHeld = true;
				escHoldTime = 0f;
			}
			else
			{
				escHoldTime += (float)delta;
				if (escHoldTime >= 3f)
				{
					GetTree().Quit();
				}
			}
		}
		else
		{
			escHeld = false;
			escHoldTime = 0f;
		}
	}

	private void ToggleDimOverlay()
	{
		// Find the DebugText label in the current scene
		var debugText = GetTree().CurrentScene?.FindChild("DebugText", true, false) as Label;
		if (debugText != null)
		{
			var mainmenu = GetTree().CurrentScene?.FindChild("MainMenu", true, false) as Control;
			var menuWidth = mainmenu.GetTransform().X;
			var menuHeight = mainmenu.GetTransform().Y;
			debugText.Text = $"Menu Size: {menuWidth} x {menuHeight}";
			// var size = GetViewport().GetVisibleRect().Size;
			// debugText.Text = $"Resolution: {size.X} x {size.Y}";
		}

		if (overlayInstance == null)
		{
			var overlayScene = GD.Load<PackedScene>("res://Scenes/Menus/Quit Game Modal.tscn");
			overlayInstance = overlayScene.Instantiate();
			AddChild(overlayInstance);

			// Clear the reference when the modal is actually removed
			overlayInstance.TreeExiting += () => overlayInstance = null;
		}
		else
		{
			overlayInstance.QueueFree();
			// Do NOT set overlayInstance = null here; let the TreeExiting signal handle it
		}
	}
}