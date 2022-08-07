using Godot;
using System;

public class Options : Control
{
	private bool active = false;
	private Tween tween;
	private bool opening = false;
	public override void _Ready()
	{
		tween = GetNode<Tween>("Tween");
		tween.Connect("tween_all_completed", this, nameof(TweenCompleted));
		var topbar = GetNode<Control>("Topbar");
		var closeBtn = topbar.GetNode<Button>("Close");
		closeBtn.Connect("pressed", this, nameof(Close));
	}
	public override void _PhysicsProcess(float delta)
	{
		if (Input.IsActionJustPressed("options"))
		{
			if (active)
				Close();
			else
				Open();
		}
	}
	public void Open()
	{
		if (this.Visible)
			return;
		opening = true;
		this.Visible = true;
		tween.InterpolateProperty(this, "modulate:a", 0, 1, 0.15f, Tween.TransitionType.Sine);
		tween.InterpolateProperty(this, "rect_scale", new Vector2(0.8f, 0.8f), new Vector2(1, 1), 0.2f, Tween.TransitionType.Sine, Tween.EaseType.Out);
		tween.Start();
	}
	public void Close()
	{
		if (!this.Visible)
			return;
		active = false;
		opening = false;
		tween.InterpolateProperty(this, "modulate:a", 1, 0, 0.15f, Tween.TransitionType.Sine);
		tween.InterpolateProperty(this, "rect_scale", new Vector2(1, 1), new Vector2(0.9f, 0.9f), 0.2f, Tween.TransitionType.Sine, Tween.EaseType.Out);
		tween.Start();
	}
	private void TweenCompleted()
	{
		tween.RemoveAll();
		if (!opening)
		{
			this.Visible = false;
		}
		else
		{
			active = true;
		}
	}
}
