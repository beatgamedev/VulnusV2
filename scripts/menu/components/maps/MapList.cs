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
	private int offset = 0;
	private int visible = 0;
	private int selected = 0;
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
	public void UpdateDisplayed(bool render = false)
	{
		DisplayedMaps = RootMaps.FindAll((BeatmapSet set) => { return set.Name.Match(filters.GetNode<LineEdit>("Search").Text, false); });
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
		Array.Resize(ref mapButtons, visible);
		for (int i = 0; i < visible; i++)
		{
			if (mapButtons[i] == null)
				mapButtons[i] = newButton();
			var btn = mapButtons[i];
			btn.Mapset = DisplayedMaps[offset + i];
			btn.Update();
		}
	}
}
