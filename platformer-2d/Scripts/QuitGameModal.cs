using Godot;
using System;

public partial class QuitGameModal : Control
{
	private Control previouslyFocused = null;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Vector2 windowSize = GetViewport().GetVisibleRect().Size;
		Size = windowSize;
		// Optionally, set position to (0,0) to ensure it starts at the top-left
		Position = Vector2.Zero;

		previouslyFocused = GetViewport().GuiGetFocusOwner() as Control;
	
		var myButton = GetNode<Button>("Panel/VBoxContainer/HBoxContainer/KeepPlayingBtn");
		myButton.GrabFocus();
		myButton.Pressed += OnKeepPlayingPressed;


		var quitBtn = GetNode<Button>("Panel/VBoxContainer/HBoxContainer/QuitBtn");
		quitBtn.Pressed += OnQuitPressed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnQuitPressed()
	{
		GetTree().Quit();
	}
	private void OnKeepPlayingPressed()
	{
		QueueFree(); // Destroys this instance of QuitGameModal
		previouslyFocused?.GrabFocus();
	}
}
