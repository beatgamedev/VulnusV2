using Godot;
using System;

public class MapList : Control
{
	public Map SelectedMap { get; private set; }
	private MapButton[] mapButtons;
	private VBoxContainer content;
	private float scrollSpeed = 0;
	private float scrollSensitivity = 96f;
	private float scrollPosition = 0;
	public override void _Ready()
	{
		content = GetNode<VBoxContainer>("Content");
		mapButtons = new MapButton[MapLoader.LoadedMaps.Count];
		int i = 0;
		foreach (Map map in MapLoader.LoadedMaps)
		{
			var btn = MapButton.Create();
			btn.Map = map;
			btn.ManualUpdate();
			btn.Connect("pressed", this, nameof(OnMapButtonPressed), new Godot.Collections.Array() { btn });
			content.AddChild(btn);
			mapButtons[i] = btn;
			i++;
		}
	}
	public void OnMapButtonPressed(MapButton button)
	{
		SelectedMap = button.Pressed ? button.Map : null;
		EmitSignal(nameof(MapSelected), button.Pressed ? SelectedMap.Hash : null);
		foreach (MapButton btn in mapButtons)
		{
			btn.Pressed = button.Pressed && btn == button;
		}
	}
	[Signal]
	public delegate void MapSelected(string hash);
	public override void _Process(float delta)
	{
		if (mapButtons.Length <= 1)
			return;
		scrollPosition = Mathf.Clamp(scrollPosition + (scrollSpeed * scrollSensitivity * delta / 0.2f), (-64f * MapLoader.LoadedMaps.Count) + (RectSize.y - 64f), 0f);
		content.RectPosition = new Vector2(content.RectPosition.x, scrollPosition);
		scrollSpeed -= scrollSpeed * delta / 0.2f;
	}
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton)
		{
			var ev = (InputEventMouseButton)@event;
			if (!ev.IsPressed()) return;
			if (ev.ButtonIndex == (int)ButtonList.WheelUp)
				scrollSpeed += 1;
			if (ev.ButtonIndex == (int)ButtonList.WheelDown)
				scrollSpeed -= 1;
		}
	}
}
