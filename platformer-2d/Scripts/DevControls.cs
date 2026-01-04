using Godot;
// DevControls: Handles dev overlay and quit shortcut
public partial class DevControls : Node
{
	// Variables
	private float escHoldTime;
	private bool escHeld;
	private Node overlayInstance;

	public override void _Ready()
	{
		SetProcessInput(true);
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey { Pressed: true, Keycode: Key.Escape, Echo: false })
			ToggleDimOverlay();
	}
	public override void _Process(double delta)
	{
		CheckEscHoldToQuit(delta);
	}

	// Quit if Escape held for 3 seconds
	private void CheckEscHoldToQuit(double delta)
	{
		if (Input.IsKeyPressed(Key.Escape))
		{
			escHoldTime += (float)delta;
			if (!escHeld && escHoldTime > 0f)
				escHeld = true;
			if (escHoldTime >= 3f)
				GetTree().Quit();
		}
		else
		{
			escHeld = false;
			escHoldTime = 0f;
		}
	}

	private void ToggleDimOverlay()
	{
		// Optionally update debug text with menu size
		var debugText = GetTree().CurrentScene?.FindChild("DebugText", true, false) as Label;
		var mainmenu = GetTree().CurrentScene?.FindChild("MainMenu", true, false) as Control;
		if (debugText != null && mainmenu != null)
		{
			var menuSize = mainmenu.Size;
			debugText.Text = $"Menu Size: {menuSize.X} x {menuSize.Y}";
		}

		if (overlayInstance == null)
		{
			var overlayScene = GD.Load<PackedScene>("res://Scenes/Menus/Quit Game Modal.tscn");
			overlayInstance = overlayScene.Instantiate();
			// Try to find the 'UI Elements' CanvasLayer in the current scene
			var uiLayer = GetTree().CurrentScene?.FindChild("UI Elements", true, false) as CanvasLayer;
			if (uiLayer != null && overlayInstance.GetParent() != uiLayer)
			{
				uiLayer.AddChild(overlayInstance);
			}
			else if (overlayInstance.GetParent() == null)
			{
				// Fallback: add to current scene or self
				var parent = GetTree().CurrentScene ?? this;
				parent.AddChild(overlayInstance);
			}
			overlayInstance.TreeExiting += () => overlayInstance = null;
		}
		else
		{
			overlayInstance.QueueFree();
		}
	}
}
