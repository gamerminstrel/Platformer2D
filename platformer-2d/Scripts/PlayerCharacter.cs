using Godot;
using System;

namespace Platformer2D.Scripts
{
    public partial class PlayerCharacter : CharacterBody2D
    {
        // Gravity and terminal velocity values (adjust as needed)
        [Export] public float Gravity = 2000f;
        [Export] public float TerminalVelocity = 2000f;
        [Export] public float MoveSpeed = 300f;
        [Export] public float JumpVelocity = 600f;

        private readonly Vector2 _snapVector = Vector2.Down * 8;
        private AnimatedSprite2D _sprite;
        private bool _wasMoving;
        private bool _isPlayingIdle2Jog;
        private bool _isPlayingJogToIdle;

        public bool IsOnGround { get; private set; } = false;

        public enum FacingDirection { Left, Right }
        public FacingDirection Facing { get; private set; } = FacingDirection.Right;

        // Store original scale and modulate for restoration
        private Vector2 _originalScale;
        private Color _originalModulate;
        private bool _runModeActive = false;

        public override void _Ready()
        {
            _originalScale = Scale;
            _originalModulate = Modulate;
            FloorSnapLength = 8.0f;
            FloorMaxAngle = 45.0f;
            FloorStopOnSlope = true;
            _sprite = GetNodeOrNull<AnimatedSprite2D>("CharacterAnimation");
            if (_sprite != null)
                _sprite.AnimationFinished += OnAnimationFinished;
        }

        public override void _PhysicsProcess(double delta)
        {
            Vector2 velocity = Velocity;

            // Handle left/right input
            float input = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
            velocity.X = input * MoveSpeed;

            if (input < 0)
                Facing = FacingDirection.Left;
            else if (input > 0)
                Facing = FacingDirection.Right;

            // Flip sprite based on facing direction
            if (_sprite != null)
                _sprite.FlipH = Facing == FacingDirection.Left; // BUG FIX: Use FlipH property, not Set("flip_h", ...)

            // Check if on ground using built-in method
            IsOnGround = IsOnFloor();

            // Animation logic
            bool isMoving = Math.Abs(input) > 0.01f;

            if (IsOnGround && _sprite != null)
            {
                if (!_wasMoving && isMoving)
                {
                    _sprite.Play("idle2Jog");
                    _isPlayingIdle2Jog = true;
                }
                else if (_wasMoving && !isMoving)
                {
                    _sprite.Play("idle2Jog");
                    _isPlayingJogToIdle = true;
                }
                else if (!_isPlayingIdle2Jog && !_isPlayingJogToIdle)
                {
                    if (isMoving && _sprite.Animation != "Jog")
                        _sprite.Play("Jog");
                    else if (!isMoving && _sprite.Animation != "Placeholder_idle")
                        _sprite.Play("Placeholder_idle");
                }
            }
            // Animation transitions are now handled in OnAnimationFinished

            _wasMoving = isMoving;

            // Jumping
            if (IsOnGround && Input.IsActionJustPressed("ui_accept"))
            {
                velocity.Y = -JumpVelocity;
            }
            else if (!IsOnGround)
            {
                // Apply gravity
                velocity.Y += Gravity * (float)delta;
                if (velocity.Y > TerminalVelocity)
                    velocity.Y = TerminalVelocity;
            }
            else
            {
                // Optionally reset vertical velocity when on ground
                velocity.Y = 0;
            }

            // Toggle run mode based on shift key
            if (Input.IsKeyPressed(Key.Shift))
            {
                if (!_runModeActive)
                    SetRunMode(true);
            }
            else
            {
                if (_runModeActive)
                    SetRunMode(false);
            }

            Velocity = velocity;
            MoveAndSlide(); // Godot 4.2+ handles snapping and slopes automatically
        }

        // Animation finished signal handler for smooth transitions
        private void OnAnimationFinished()
        {
            if (_sprite == null)
                return;

            string animName = _sprite.Animation;
            if (animName == "idle2Jog")
            {
                if (_isPlayingIdle2Jog)
                {
                    _sprite.Play("Jog");
                    _isPlayingIdle2Jog = false;
                }
                else if (_isPlayingJogToIdle)
                {
                    _sprite.Play("Placeholder_idle");
                    _isPlayingJogToIdle = false;
                }
            }
        }

        // Function to toggle run mode (squish/stretch and tint)
        private void SetRunMode(bool enable)
        {
            var sprite = _sprite;
            if (enable && sprite?.SpriteFrames != null)
            {
                int frameIndex = Math.Max(sprite.Frame, 0);
                Vector2 texSize = sprite.SpriteFrames.GetFrameTexture(sprite.Animation, frameIndex).GetSize();
                Vector2 newScale = _originalScale;
                newScale.X *= (texSize.X + 15f) / texSize.X;
                newScale.Y *= (texSize.Y - 15f) / texSize.Y;
                sprite.Scale = newScale;
                sprite.Modulate = new Color(1f, 0f, 0f, 1f);
                MoveSpeed = 600f;
                _runModeActive = true;
            }
            else if (sprite != null)
            {
                sprite.Scale = _originalScale;
                sprite.Modulate = _originalModulate;
                MoveSpeed = 300f;
                _runModeActive = false;
            }
        }
    }
}