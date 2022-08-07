using Godot;
using System;

public class Debug : Control
{
	private int frames = 0;
	private int countFrom = (int)OS.GetTicksMsec();
	private Label renderLabel;
	private Label physicsLabel;
	public override void _Ready()
	{
		renderLabel = GetNode<Label>("Render").GetNode<Label>("Value");
		physicsLabel = GetNode<Label>("Physics").GetNode<Label>("Value");
	}
	public override void _Process(float delta)
	{
		renderLabel.Text = $"{Engine.GetFramesPerSecond()}";
	}
	public override void _PhysicsProcess(float delta)
	{
		var now = (int)OS.GetTicksMsec();
		if (now - countFrom > 1000)
		{
			physicsLabel.Text = $"{frames}";
			frames = 0;
			countFrom = now;
		}
		frames++;
	}
}
