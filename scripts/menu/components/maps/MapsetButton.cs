using Godot;
using System;

public class MapsetButton : Control
{
	public BeatmapSet Mapset;
	public bool Expanded { get; private set; }
	public Action<Beatmap> MapSelected = (Beatmap map) => { };
	private Control list;
	private Control origin;
	private Button setButton;
	private Tween tween;
	private Tween inTween;
	public override void _Ready()
	{
		tween = GetNode<Tween>("Tween");
		inTween = GetNode<Tween>("InTween");
		setButton = GetNode<Button>("Button");
		list = setButton.GetNode<VBoxContainer>("Maps");
		origin = list.GetNode<Button>("Map");
		origin.Visible = false;
		Collapse();
	}
	public void MoveIn(float delay = 0.1f)
	{
		setButton.Modulate = new Color(1, 1, 1, 0);
		inTween.RemoveAll();
		inTween.InterpolateProperty(setButton, "rect_position:x", 48, 0, 0.4f, Tween.TransitionType.Expo, Tween.EaseType.Out, delay);
		inTween.InterpolateProperty(setButton, "modulate:a", 0, 1, 0.3f, Tween.TransitionType.Expo, Tween.EaseType.Out, delay);
		inTween.Start();
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
		setButton.GetNode<TextureRect>("Cover/Image").Texture = Mapset.LoadCover();
		setButton.GetNode<Label>("Title").Text = Mapset.Title;
		setButton.GetNode<Label>("Title/Artist").Text = Mapset.Artist;
		setButton.GetNode<Label>("Title/Mapper").Text = Mapset.Mappers;
		if (resetButtons)
			ResetButtons();
	}
	public void Expand(bool animate = false)
	{
		Expanded = true;
		tween.RemoveAll();
		if (!animate)
		{
			RectMinSize = new Vector2(0, 20 + (list.GetChildCount() * 56));
			list.Modulate = new Color(1, 1, 1, 1);
			return;
		}
		tween.InterpolateProperty(list, "modulate:a", 0, 1, 0.35f, Tween.TransitionType.Expo, Tween.EaseType.Out);
		tween.InterpolateProperty(this, "rect_min_size:y", 76, 20 + (list.GetChildCount() * 56), 0.4f, Tween.TransitionType.Expo, Tween.EaseType.Out);
		tween.Start();
	}
	public void Collapse(bool animate = false)
	{
		Expanded = false;
		tween.RemoveAll();
		if (!animate)
		{
			RectMinSize = new Vector2(0, 76);
			list.Modulate = new Color(1, 1, 1, 0);
			return;
		}
		tween.InterpolateProperty(list, "modulate:a", 1, 0, 0.35f, Tween.TransitionType.Expo, Tween.EaseType.Out);
		tween.InterpolateProperty(this, "rect_min_size:y", 20 + (list.GetChildCount() * 56), 76, 0.4f, Tween.TransitionType.Expo, Tween.EaseType.Out);
		tween.Start();
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
