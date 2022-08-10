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
		foreach (View view in views)
		{
			view.Visible = false;
		}
		Views = views;
		CallDeferred(nameof(GoTo), 0);
	}
	public void GoTo(int view)
	{
		if (view != VisibleView)
			Views[VisibleView].SetActive(false);
		VisibleView = view;
		Views[view].SetActive(true);
	}
}
