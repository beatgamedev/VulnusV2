using Godot;
using System;
using System.Collections.Generic;

public class MapList : Control
{
	public static string Search = "";
	public static int Sort = 0;
	public List<BeatmapSet> RootMaps = BeatmapLoader.LoadedMaps;
	public List<BeatmapSet> DisplayedMaps { get; private set; }
	public BeatmapSet SelectedMapset;
	public Beatmap SelectedMap;
	private MapsetButton origin;
	private Dictionary<BeatmapSet, MapsetButton> mapButtons = new Dictionary<BeatmapSet, MapsetButton>();
	private Control content;
	private Control anchor;
	private Control list;
	private Control filters;
	private HSeparator separator;
	private float scroll = 0;
	private float scrollf = 0;
	public int visible { get; private set; } = 0;
	public Action<Beatmap> MapSelected = (Beatmap map) => { };
	public Action<BeatmapSet> MapsetSelected = (BeatmapSet mapset) => { };
	public override void _Ready()
	{
		content = GetNode<Control>("Content");
		anchor = content.GetNode<Control>("Anchor");
		list = anchor.GetNode<Control>("List");
		separator = list.GetNode<HSeparator>("Separator");
		origin = list.GetNode<MapsetButton>("Mapset");
		origin.Visible = false;
		filters = GetNode<Control>("Filters");
		filters.GetNode<LineEdit>("Search").Connect("text_changed", this, nameof(SearchChanged));
		filters.GetNode<LineEdit>("Search").Text = Search;
		filters.GetNode<OptionButton>("Sort").Connect("item_selected", this, nameof(SortChanged));
		filters.GetNode<OptionButton>("Sort").Selected = Sort;
	}
	public override void _Process(float delta)
	{
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
	public void Scroll(float amount, bool render = true)
	{
		if (amount == 0)
			return;
		if (scroll + amount < 0 || scroll + amount > DisplayedMaps.Count)
			return;
		scroll += amount;
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
		if (SelectedMapset == btn.Mapset)
			return;
		if (SelectedMapset != null && mapButtons.ContainsKey(SelectedMapset))
			mapButtons[SelectedMapset].Collapse(true);
		btn.ManualUpdate(true);
		btn.Expand(true);
		SelectedMapset = btn.Mapset;
		MapsetSelected(SelectedMapset);
	}
	public void SearchChanged(string search)
	{
		Search = search;
		UpdateDisplayed(true);
	}
	public void SortChanged(int index)
	{
		Sort = index;
		UpdateDisplayed(true);
	}
	private bool IsSimilar(BeatmapSet set, string search)
	{
		var name = set.Name.ToLower().Trim();
		var mappers = set.Mappers.ToLower().Trim();
		search = search.ToLower().Trim();
		return (name.Contains(search)
			|| name.Similarity(search) >= 0.5)
			|| (mappers.Contains(search)
			|| mappers.Similarity(search) >= 0.5);
	}
	public void UpdateDisplayed(bool render = false)
	{
		scroll = 0;
		var search = filters.GetNode<LineEdit>("Search").Text.Trim();
		if (search != "")
			DisplayedMaps = RootMaps.FindAll((BeatmapSet set) => IsSimilar(set, search));
		else
			DisplayedMaps = RootMaps;
		switch (filters.GetNode<OptionButton>("Sort").Selected)
		{
			case 0: // Title (asc.)
				DisplayedMaps.Sort(delegate (BeatmapSet a, BeatmapSet b)
				{
					return a.Title.CompareTo(b.Title);
				});
				break;
			case 1: // Title (dsc.)
				DisplayedMaps.Sort(delegate (BeatmapSet a, BeatmapSet b)
				{
					return -a.Title.CompareTo(b.Title);
				});
				break;
			case 2: // Artist (asc.)
				DisplayedMaps.Sort(delegate (BeatmapSet a, BeatmapSet b)
				{
					return a.Name.CompareTo(b.Name);
				});
				break;
			case 3: // Artist (dsc.)
				DisplayedMaps.Sort(delegate (BeatmapSet a, BeatmapSet b)
				{
					return -a.Name.CompareTo(b.Name);
				});
				break;
			// UNIMPLEMENTED BELOW
			case 5: // Difficulty (asc.)
					// DisplayedMaps.Sort(delegate (BeatmapSet a, BeatmapSet b)
					// {
					// 	return a.Artist.CompareTo(b.Artist);
					// });
				break;
			case 6: // Difficulty (dsc.)
					// DisplayedMaps.Sort(delegate (BeatmapSet a, BeatmapSet b)
					// {
					// 	return -a.Artist.CompareTo(b.Artist);
					// });
				break;
			default:
				break;
		}
		if (!render)
			return;
		RenderButtons();
	}
	public void RenderButtons()
	{
		int offset = (int)Math.Ceiling(scroll);
		visible = Math.Min(DisplayedMaps.Count - offset, (int)Math.Ceiling(content.RectSize.y / 72));
		List<BeatmapSet> forRemoval = new List<BeatmapSet>();
		foreach (KeyValuePair<BeatmapSet, MapsetButton> pair in mapButtons)
		{
			var set = pair.Key;
			var btn = pair.Value;
			if (DisplayedMaps.Contains(set))
				continue;
			forRemoval.Add(set);
		}
		int underAmount = SelectedMapset != null && mapButtons.ContainsKey(SelectedMapset) ? Mathf.CeilToInt(mapButtons[SelectedMapset].RectMinSize.y / 76f) : 1;
		int hierarchyIndex = 1;
		int creationIndex = 0;
		int firstIndex = -1;
		bool selectedUnder = false;
		foreach (BeatmapSet set in DisplayedMaps)
		{
			int index = DisplayedMaps.IndexOf(set);
			bool under = index < offset - underAmount;
			bool over = index > offset + visible;
			bool exists = mapButtons.ContainsKey(set) && !forRemoval.Contains(set);
			bool selected = set == SelectedMapset || (exists && mapButtons[set].RectMinSize.y != 76);
			bool skipCreation = false;
			if (selected && under)
				selectedUnder = true;
			if (exists && !selected && (under || over))
				forRemoval.Add(set);
			else if (exists)
			{
				list.MoveChild(mapButtons[set], hierarchyIndex);
				hierarchyIndex++;
			}
			if (under)
				firstIndex = index;
			skipCreation = ((over || under) && !selected) || exists;
			if (skipCreation)
				continue;
			creationIndex++;
			mapButtons[set] = newButton();
			mapButtons[set].Mapset = set;
			mapButtons[set].ManualUpdate(true);
			mapButtons[set].MoveIn((creationIndex + 1) / 20f);
			if (SelectedMapset == set)
				mapButtons[set].Expand();
			list.MoveChild(mapButtons[set], hierarchyIndex);
			hierarchyIndex++;
		}
		foreach (BeatmapSet set in forRemoval)
		{
			mapButtons[set].QueueFree();
			mapButtons.Remove(set);
		}
		if (selectedUnder)
			firstIndex--;
		separator.AddConstantOverride("separation", (firstIndex + 1) * 78);
	}
}
