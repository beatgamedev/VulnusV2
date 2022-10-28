using Godot;
using System;

public class MapList : Control
{
	public BeatmapSet SelectedMap { get; private set; }
	private MapsetButton origin;
	private MapsetButton[] mapButtons;
	private Control content;
	private Control anchor;
	public override void _Ready()
	{
		content = GetNode<Control>("Content");
		anchor = content.GetNode<Control>("Anchor");
		origin = anchor.GetNode<MapsetButton>("Mapset");
		origin.Visible = false;
		mapButtons = new MapsetButton[BeatmapLoader.LoadedMaps.Count];
		int i = 0;
		foreach (BeatmapSet map in BeatmapLoader.LoadedMaps)
		{
			MapsetButton btn = (MapsetButton)origin.Duplicate();
			btn.Visible = true;
			btn.Mapset = map;
			btn.ManualUpdate();
			anchor.AddChild(btn);
			btn.RectPosition = btn.RectPosition + new Vector2(0,72*i);
			mapButtons[i] = btn;
			i++;
		}
	}
	[Signal]
	public delegate void MapsetSelected(BeatmapSet mapset);
	[Signal]
	public delegate void MapSelected(Beatmap map);
}
