using Godot;
using System;

public class Singleplayer : View
{
	private MapList mapList;
	private Control bottomBar;
	private Tween tween;
	private MenuHandler menu;
	public override void _Ready()
	{
		base._Ready();
		menu = GetParent<Control>().GetParent<MenuHandler>();
		tween = GetNode<Tween>("Tween");
		mapList = GetNode<MapList>("MapList");
		bottomBar = GetNode<Control>("BottomBar");
		var backBtn = bottomBar.GetNode<Button>("Back");
		backBtn.Connect("pressed", menu, nameof(MenuHandler.GoTo), new Godot.Collections.Array(0));
		if (Game.Score == null)
			return;
		GetNode<MapDetails>("MapDetails").OnMapSelected(Game.LoadedMapset.Hash);
		GetNode<MapDetails>("MapDetails").OnDifficultySelected(Game.LoadedMapset.Difficulties.IndexOf(Game.LoadedMap));
	}
	public override void OnShow()
	{
		base.OnShow();
		tween.StopAll();
		tween.InterpolateProperty(mapList, "anchor_left", 0.8, 0.5, 0.3f, Tween.TransitionType.Quart, Tween.EaseType.Out);
		tween.InterpolateProperty(bottomBar, "margin_top", 0, -48, 0.2f, Tween.TransitionType.Sine, Tween.EaseType.Out);
		tween.Start();
	}
}
