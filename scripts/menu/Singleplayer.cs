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
	}
	public override void OnShow()
	{
		// base.OnShow();
		mapList.UpdateDisplayed(false);
		if (Game.Score != null)
		{
			mapList.SelectedMapset = Game.LoadedMapset;
			mapList.SelectedMap = Game.LoadedMap;
			mapList.RenderButtons();
			mapList.MapSelected(Game.LoadedMap);
			mapList.Scroll(mapList.DisplayedMaps.IndexOf(Game.LoadedMapset) - (Mathf.Floor(mapList.visible / 2f) - 2), true);
		}
		else
			mapList.RenderButtons();
		tween.StopAll();
		tween.InterpolateProperty(mapList, "modulate:a", 0, 1, 0.3f, Tween.TransitionType.Quart, Tween.EaseType.Out);
		tween.InterpolateProperty(bottomBar, "margin_top", 0, -48, 0.2f, Tween.TransitionType.Sine, Tween.EaseType.Out);
		tween.Start();
		this.Visible = true;
	}
	public override async void OnHide()
	{
		// base.OnHide();
		tween.StopAll();
		tween.InterpolateProperty(mapList, "modulate:a", 1, 0, 0.3f, Tween.TransitionType.Quart, Tween.EaseType.Out);
		tween.InterpolateProperty(bottomBar, "margin_top", -48, 0, 0.2f, Tween.TransitionType.Sine, Tween.EaseType.Out);
		tween.Start();
		await ToSignal(tween, "tween_all_completed");
		this.Visible = false;
	}
}
