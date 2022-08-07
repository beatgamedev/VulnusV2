using Godot;
using System;

public class MenuHandler : Control
{
	private Control[] Views;
	public override void _Ready()
	{
		var viewContainer = GetNode<Control>("ViewContainer");
		Control[] views = { viewContainer.GetNode<Control>("MainMenu") };
		Views = views;
		GoTo(0);
	}
	public void GoTo(int view)
	{
		for (int i = 0; i < Views.Length; i++)
		{
			Views[i].Visible = i == view;
		}
	}
}
