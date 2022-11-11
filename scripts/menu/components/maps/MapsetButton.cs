using Godot;
using System;

public class MapsetButton : Button
{
	public BeatmapSet Mapset;
	public bool Expanded { get; private set; }
	private Control list;
	private Control origin;
	public Action<Beatmap> MapSelected = (Beatmap map) => { };
	public override void _Ready()
	{
		list = GetNode<VBoxContainer>("Maps");
		origin = list.GetNode<Button>("Map");
		origin.Visible = false;
		Collapse();
	}
	private void btnPressed(Button button)
	{
		Beatmap map = Mapset.Difficulties[button.Name.ToInt()];
		MapSelected(map);
	}
	public void ManualUpdate(bool resetButtons = false)
	{
		if (Mapset == null)
			return;
		GetNode<TextureRect>("Cover/Image").Texture = Mapset.LoadCover();
		GetNode<Label>("Title").Text = Mapset.Title;
		GetNode<Label>("Title/Artist").Text = Mapset.Artist;
		GetNode<Label>("Title/Mapper").Text = Mapset.Mappers;
		if (resetButtons)
			ResetButtons();
	}
	public void Expand(bool animate = false)
	{
		Expanded = true;
		list.Visible = true;
	}
	public void Collapse(bool animate = false)
	{
		Expanded = false;
		list.Visible = false;
	}
	private void ResetButtons()
	{
		int children = list.GetChildCount() - 1;
		for (int i = 0; i < children; i++)
		{
			if (i >= Mapset.Difficulties.Count)
				list.GetChild<Node>(i).QueueFree();
		}
		for (int i = 0; i < Mapset.Difficulties.Count; i++)
		{
			Button btn;
			if (i >= children)
			{
				btn = (Button)origin.Duplicate();
				btn.Connect("pressed", this, nameof(btnPressed), new Godot.Collections.Array(btn));
				btn.Visible = true;
				list.AddChild(btn);
			}
			else
				btn = list.GetChild<Button>(i + 1);
			var difficulty = Mapset.Difficulties[i];
			btn.Name = i.ToString();
			btn.GetNode<Label>("Title").Text = difficulty.Name;
			btn.GetNode<TextureRect>("Cover").Texture = Mapset.Cover;
		}
	}
}
