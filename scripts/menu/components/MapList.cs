using Godot;
using System;

public class MapList : Control
{
	private MapButton[] mapButtons;
	private VBoxContainer content;
	private float scrollSpeed = 0;
	private float scrollSensitivity = 96f;
	private float scrollPosition = 0;
	public override void _Ready()
	{
		content = GetNode<VBoxContainer>("Content");
		foreach (Map map in MapLoader.LoadedMaps)
		{
			var btn = MapButton.Create();
			btn.Map = map;
			btn.ManualUpdate();
			content.AddChild(btn);
		}
	}
	public override void _Process(float delta)
	{
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
