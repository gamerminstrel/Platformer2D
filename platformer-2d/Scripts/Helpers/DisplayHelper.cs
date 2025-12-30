using Godot;
using System;

public static class DisplayHelper
{
    public static void ResizeToWindow(Control control)
    {
        control.AnchorLeft = 0;
        control.AnchorTop = 0;
        control.AnchorRight = 1;
        control.AnchorBottom = 1;
        control.OffsetLeft = 0;
        control.OffsetTop = 0;
        control.OffsetRight = 0;
        control.OffsetBottom = 0;
        control.SizeFlagsHorizontal = Control.SizeFlags.Expand;
        control.SizeFlagsVertical = Control.SizeFlags.Expand;
    }
}