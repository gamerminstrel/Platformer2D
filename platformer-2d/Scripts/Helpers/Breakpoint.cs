using Godot;

[GlobalClass]
public partial class Breakpoint : Resource
{
    [Export] public string Name { get; set; } = "";
    [Export] public int Width { get; set; } = 0;
}