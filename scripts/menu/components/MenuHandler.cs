using Godot;
using System;

public class MenuHandler : Control
{
	public static int NextView = 0;
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
		Global.Discord.SetActivity(new Discord.ActivityW(
			state: "Selecting a map",
			startTimestamp: DateTime.Now
		));
	}
	public override void _EnterTree()
	{
		base._EnterTree();
		if (NextView != 0)
		{
			CallDeferred(nameof(GoTo), NextView);
			NextView = 0;
		}
	}
	public void GoTo(int view)
	{
		if (view != VisibleView)
			Views[VisibleView].SetActive(false);
		VisibleView = view;
		Views[view].SetActive(true);
	}
}
