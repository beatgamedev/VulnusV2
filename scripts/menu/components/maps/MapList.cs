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
	private Dictionary<BeatmapSet, MapsetButton> mapButtons = new Dictionary<BeatmapSet, MapsetButton>();
	private Control content;
	private Control anchor;
	private Control list;
	private Control filters;
	private float scroll = 0;
	private float scrollf = 0;
	private float scrolls = 0;
	private int visible = 0;
	public Action<Beatmap> MapSelected = (Beatmap map) => { };
	public Action<BeatmapSet> MapsetSelected = (BeatmapSet mapset) => { };
	public override void _Ready()
	{
		content = GetNode<Control>("Content");
		anchor = content.GetNode<Control>("Anchor");
		list = anchor.GetNode<Control>("List");
		origin = list.GetNode<MapsetButton>("Mapset");
		origin.Visible = false;
		filters = GetNode<Control>("Filters");
		filters.GetNode<LineEdit>("Search").Connect("text_changed", this, nameof(SearchChanged));
	}
	public override void _Process(float delta)
	{
		if (mapButtons.Count == 0 && RootMaps.Count > 0)
			UpdateDisplayed(true);
		scrollf += (scroll - scrollf) * delta / 0.1f;
		anchor.RectPosition = new Vector2(anchor.RectPosition.x, -scrollf * 78);
		base._Process(delta);
	}
	public override void _GuiInput(InputEvent @event)
	{
		if (!(@event is InputEventMouseButton))
			return;
		var ev = (InputEventMouseButton)@event;
		if (!ev.Pressed)
			return;
		if (ev.ButtonIndex == (int)ButtonList.WheelUp)
			Scroll(-1);
		else if (ev.ButtonIndex == (int)ButtonList.WheelDown)
			Scroll(1);
	}
	public void Scroll(float amount)
	{
		if (scroll + amount < 0 || scroll + amount > DisplayedMaps.Count)
			return;
		scroll += amount;
		scrolls += amount;
		RenderButtons();
	}
	private MapsetButton newButton()
	{
		MapsetButton newBtn = origin.Duplicate() as MapsetButton;
		newBtn.MapSelected += mapSelected;
		newBtn.GetNode<Button>("Button").Connect("pressed", this, nameof(btnPressed), new Godot.Collections.Array(newBtn));
		newBtn.Visible = true;
		list.AddChild(newBtn);
		return newBtn;
	}
	private void mapSelected(Beatmap map)
	{
		SelectedMap = map;
		MapSelected(SelectedMap);
	}
	private void btnPressed(MapsetButton btn)
	{
		if (SelectedMapset != null && mapButtons.ContainsKey(SelectedMapset))
			mapButtons[SelectedMapset].Collapse(true);
		btn.ManualUpdate(true);
		btn.Expand(true);
		SelectedMapset = btn.Mapset;
		MapsetSelected(SelectedMapset);
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
		scroll = 0;
		scrollf = 0;
		var search = filters.GetNode<LineEdit>("Search").Text.Trim();
		if (search != "")
			DisplayedMaps = RootMaps.FindAll((BeatmapSet set) => IsSimilar(set, search));
		else
			DisplayedMaps = RootMaps;
		if (!render)
			return;
		RenderButtons();
	}
	public void RenderButtons()
	{
		int offset = (int)Math.Ceiling(scroll);
		visible = Math.Min(DisplayedMaps.Count - offset, (int)Math.Ceiling(content.RectSize.y / 72));
		foreach (KeyValuePair<BeatmapSet, MapsetButton> pair in mapButtons)
		{
			var set = pair.Key;
			var btn = pair.Value;
			btn.Visible = DisplayedMaps.Contains(set);
		}
		foreach (BeatmapSet set in DisplayedMaps)
		{
			int index = DisplayedMaps.IndexOf(set);
			if (!(index >= offset && index <= offset + visible))
				continue;
			if (mapButtons.ContainsKey(set))
			{
				list.MoveChild(mapButtons[set], index);
				continue;
			}
			mapButtons[set] = newButton();
			mapButtons[set].Mapset = set;
			mapButtons[set].ManualUpdate(true);
		}
	}
}
