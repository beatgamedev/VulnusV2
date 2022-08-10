using Godot;
using System;

public class Singleplayer : View
{
	private Control mapList;
	private Control mapDetail;
	private Control bottomBar;
	private Tween tween;
	private MenuHandler menu;
	public override void _Ready()
	{
		menu = GetParent<Control>().GetParent<MenuHandler>();
		tween = GetNode<Tween>("Tween");
		// mapList = GetNode<Control>("MapList");
		// mapDetail = GetNode<Control>("MapDetail");
		bottomBar = GetNode<Control>("BottomBar");
		var backBtn = bottomBar.GetNode<Button>("Back");
		backBtn.Connect("pressed", menu, nameof(MenuHandler.GoTo), new Godot.Collections.Array(0));
	}
	public override void OnShow()
	{
		base.OnShow();
		tween.StopAll();
		tween.InterpolateProperty(bottomBar, "margin_top", 0, -48, 0.2f, Tween.TransitionType.Sine, Tween.EaseType.Out);
		tween.InterpolateProperty(bottomBar, "modulate:a", 0, 1, 0.15f, Tween.TransitionType.Sine, Tween.EaseType.Out);
		tween.Start();
	}
}
