using Godot;
using System;

public class Debug : Control
{
	private int frames = 0;
	private int countFrom = (int)OS.GetTicksMsec();
	private Label renderLabel;
	public override void _Ready()
	{
		renderLabel = GetNode<Label>("Render").GetNode<Label>("Value");
	}
	public override void _Process(float delta)
	{
		var now = (int)OS.GetTicksMsec();
		if (now - countFrom > 1000)
		{
			// renderLabel.Text = $"{frames}";
			frames = 0;
			countFrom = now;
		}
		frames++;
		renderLabel.Text = $"{Engine.GetFramesPerSecond()}";
	}
}
