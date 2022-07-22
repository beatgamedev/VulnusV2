using Godot;
using System;

public class MainMenuController : ViewController
{
	public override void _Ready()
	{
		this.ShowView(this.FindNode("Play") as View);
	}
}
