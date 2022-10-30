using Godot;
using System;

public class MapList : Control
{
	public BeatmapSet SelectedMapset { get; private set; }
	public Beatmap SelectedMap { get; private set; }
	private MapsetButton origin;
	private MapsetButton[] mapButtons = new MapsetButton[0];
	private Control content;
	private Control anchor;
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
	}
	public void RenderButtons() {
		
	}
}
