using Godot;
using System;

public class MenuHandler : Control
{
	private int VisibleView = 0;
	private View[] Views;
	public override void _Ready()
	{
		var viewContainer = GetNode<Control>("ViewContainer");
		View[] views = { viewContainer.GetNode<View>("MainMenu"), viewContainer.GetNode<View>("Singleplayer") };
		Views = views;
		GoTo(0);
	}
	public void GoTo(int view)
	{
		Views[VisibleView].SetActive(false);
		Views[view].SetActive(true);
		VisibleView = view;
	}
}
