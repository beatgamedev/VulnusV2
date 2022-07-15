using Godot;
using System;

public class Play : View
{
	public override void Display(Control container)
	{
		base.Display(container);
		GetTree().CurrentScene = this;
	}
}
