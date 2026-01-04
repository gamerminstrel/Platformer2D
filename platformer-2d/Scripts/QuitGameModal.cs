using Godot;

namespace Platformer2D.Scripts
{
	public partial class QuitGameModal : Control
	{
		private Control previouslyFocused;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			// Move this modal to a CanvasLayer on the root viewport to guarantee it covers the screen
			var root = GetTree().Root;
			CanvasLayer modalLayer = null;
			foreach (var child in root.GetChildren())
			{
				if (child is CanvasLayer cl && cl.Name == "ModalLayer")
				{
					modalLayer = cl;
					break;
				}
			}
			if (modalLayer == null)
			{
				modalLayer = new CanvasLayer { Name = "ModalLayer", Layer = 128 };
				root.AddChild(modalLayer);
			}
			if (GetParent() != modalLayer)
				modalLayer.AddChild(this);
			modalLayer.Layer = 128; // Ensure always on top

			SetAnchorsAndOffsetsPreset(LayoutPreset.FullRect);
			UpdateModalSize();
			GetViewport().SizeChanged += UpdateModalSize;

			previouslyFocused = GetViewport().GuiGetFocusOwner();

			var keepPlayingBtn = GetNode<Button>("Panel/VBoxContainer/HBoxContainer/KeepPlayingBtn");
			keepPlayingBtn.GrabFocus();
			keepPlayingBtn.Pressed += OnKeepPlayingPressed;
			GetNode<Button>("Panel/VBoxContainer/HBoxContainer/QuitBtn").Pressed += OnQuitPressed;

			GetTree().Paused = true;
		}

		public override void _EnterTree()
		{
			// Ensure full screen and centered as soon as added to scene
			SetAnchorsAndOffsetsPreset(LayoutPreset.FullRect);
			UpdateModalSize();
		}

		public override void _Input(InputEvent @event)
		{
			if (@event is InputEventKey { Pressed: true, Keycode: Key.Escape, Echo: false })
			{
				OnKeepPlayingPressed();
				GetViewport().SetInputAsHandled();
			}
		}

		private void UpdateModalSize()
		{
			var rect = GetViewport().GetVisibleRect();
			SetAnchorsAndOffsetsPreset(LayoutPreset.FullRect);
			Size = rect.Size;
			Position = rect.Position;
		}

		private void OnQuitPressed()
			=> GetTree().Quit();

		private void OnKeepPlayingPressed()
		{
			GetTree().Paused = false;
			QueueFree();
			previouslyFocused?.GrabFocus();
		}
	}
}
