using Godot;
using System;

public partial class MainMenu : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Vector2 windowSize = GetViewport().GetVisibleRect().Size;
		Size = windowSize;

		var vbox = GetNode<VBoxContainer>("VBoxContainer");

		var newGameBtn = vbox.GetNode<Button>("NewGameBtn");
		var loadGameBtn = vbox.GetNode<Button>("LoadGameBtn");
		var optionsBtn = vbox.GetNode<Button>("OptionsBtn");
		var quitGameBtn = vbox.GetNode<Button>("QuitGameBtn");


		newGameBtn.Pressed += OnNewGamePressed;
		loadGameBtn.Pressed += OnLoadGamePressed;
		optionsBtn.Pressed += OnOptionsPressed;
		quitGameBtn.Pressed += OnQuitGamePressed;

		// Indent buttons in VBoxContainer
		// int baseIndent = 0;
		// int indentStep = 20; // pixels to indent each subsequent button

		// Get the VBoxContainer by name or index
		// var vbox = GetNode<VBoxContainer>("VBoxContainer");
		// for (int i = 0; i < vbox.GetChildCount(); i++)
		// {
		// 	if (vbox.GetChild(i) is Button button)
		// 	{
		// 		button.CustomMinimumSize = new Vector2(baseIndent + i * indentStep, button.CustomMinimumSize.Y);
		// 		button.AddThemeConstantOverride("margin_left", baseIndent + i * indentStep);
		// 	}
		// }

		var defaultButton = GetNode<Button>("VBoxContainer/LoadGameBtn");
		defaultButton.GrabFocus();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnNewGamePressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/PlatformingRoom1.tscn");

	}

	private void OnLoadGamePressed()
	{
		// TODO: Implement load game logic
	}

	private void OnOptionsPressed()
	{
		// TODO: Implement options logic
	}

	private void OnQuitGamePressed()
	{
		GetTree().Quit();
	}
}
