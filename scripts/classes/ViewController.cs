using Godot;
using System;

public class ViewController : Control
{
	public View CurrentView { get; private set; }
	public Control ViewContainer { get; private set; }
	public override void _Ready()
	{
		ViewContainer = GetNode<Control>("ViewContainer");
	}
	public void ShowView(View view)
	{
		if (CurrentView != null)
		{
			CurrentView.Hide();
		}
		CurrentView = view;
		CurrentView.Display(ViewContainer);
	}
}
