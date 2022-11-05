using Godot;
using System;
using System.Collections.Generic;

public class MapList : Control
{
	public List<BeatmapSet> RootMaps = BeatmapLoader.LoadedMaps;
	public List<BeatmapSet> DisplayedMaps { get; private set; }
	public BeatmapSet SelectedMapset { get; private set; }
	public Beatmap SelectedMap { get; private set; }
	private MapsetButton origin;
	private MapsetButton[] mapButtons = new MapsetButton[0];
	private Control content;
	private Control anchor;
	private Control filters;
	private float scroll = 0;
	private float scrollf = 0;
	private int offset = 0;
	private int visible = 0;
	[Signal]
	public delegate void MapsetSelected(BeatmapSet mapset);
	[Signal]
	public delegate void MapSelected(Beatmap map);
	public override void _Ready()
	{
		content = GetNode<Control>("Content");
		anchor = content.GetNode<Control>("Anchor");
		origin = anchor.GetNode<MapsetButton>("Mapset");
		origin.Visible = false;
		filters = GetNode<Control>("Filters");
		filters.GetNode<LineEdit>("Search").Connect("text_changed", this, nameof(SearchChanged));
		RootMaps = BeatmapLoader.LoadedMaps;
	}
	public override void _Process(float delta)
	{
		anchor.RectPosition = new Vector2(anchor.RectPosition.x, scrollf * 72);
		base._Process(delta);
	}
	public override void _GuiInput(InputEvent @event)
	{
		if (!(@event is InputEventMouseButton))
			return;
		var ev = (InputEventMouseButton)@event;
		if (ev.ButtonIndex == (int)ButtonList.WheelUp)
			Scroll(-1f);
		else if (ev.ButtonIndex == (int)ButtonList.WheelDown)
			Scroll(1f);
	}
	public void Scroll(float amount)
	{
		var tween = anchor.GetNode<Tween>("Tween");
		tween.StopAll();
		tween.InterpolateProperty(this, nameof(scrollf), scrollf, scroll + amount, 0.2f, Tween.TransitionType.Cubic);
		scroll += amount;
		tween.Start();
	}
	private MapsetButton newButton()
	{
		MapsetButton newBtn = origin.Duplicate() as MapsetButton;
		newBtn.Visible = true;
		anchor.AddChild(newBtn);
		return newBtn;
	}
	public void SearchChanged(string search)
	{
		UpdateDisplayed(true);
	}
	private bool IsSimilar(BeatmapSet set, string search)
	{
		var name = set.Name.ToLower().Trim();
		search = search.ToLower().Trim();
		return name.Contains(search)
			|| name.Similarity(search) >= 0.5;
	}
	public void UpdateDisplayed(bool render = false)
	{
		var search = filters.GetNode<LineEdit>("Search").Text.Trim();
		if (search != "")
			DisplayedMaps = RootMaps.FindAll((BeatmapSet set) => IsSimilar(set, search));
		else
			DisplayedMaps = RootMaps;
		GD.Print(search, DisplayedMaps.Count);
		if (!render)
			return;
		RenderButtons();
	}
	public void Expand(MapsetButton button, bool animate = true)
	{

	}
	public void Collapse(MapsetButton button, bool animate = true)
	{

	}
	public void RenderButtons()
	{
		visible = Math.Min(DisplayedMaps.Count - offset, (int)Math.Ceiling(content.RectSize.y / 72));
		for (int i = 0; i < mapButtons.Length; i++)
		{
			if (i >= visible)
				mapButtons[i].QueueFree();
		}
		Array.Resize(ref mapButtons, visible);
		for (int i = 0; i < visible; i++)
		{
			if (mapButtons[i] == null)
				mapButtons[i] = newButton();
			var btn = mapButtons[i];
			btn.Mapset = DisplayedMaps[offset + i];
			if (btn.Mapset == SelectedMapset)
				Expand(btn, false);
			btn.ManualUpdate(true);
			btn.RectPosition += new Vector2(0, (offset + i) * 72) - new Vector2(0, btn.RectPosition.y);
		}
	}
}
