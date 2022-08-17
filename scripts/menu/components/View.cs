using Godot;
using System;

public partial class View : Control
{
	private Tween tween;
	public override void _Ready()
	{
		tween = new Tween();
		AddChild(tween);
	}
	public bool IsActive { get; protected set; } = false;
	public void SetActive(bool active)
	{
		if (active)
			this.OnShow();
		else
			this.OnHide();
	}
	public virtual async void OnShow()
	{
		this.Visible = true;
		tween.StopAll();
		this.RectPosition = new Vector2(0, 0);
		tween.InterpolateProperty(this, "modulate:a", 0, 1, 0.15f, Tween.TransitionType.Sine, Tween.EaseType.Out);
		tween.Start();
		await ToSignal(tween, "tween_all_completed");
		this.IsActive = true;
	}
	public virtual async void OnHide()
	{
		this.IsActive = false;
		tween.StopAll();
		tween.InterpolateProperty(this, "rect_position:y", 0, 64, 0.15f, Tween.TransitionType.Sine, Tween.EaseType.Out);
		tween.InterpolateProperty(this, "modulate:a", 1, 0, 0.15f, Tween.TransitionType.Sine, Tween.EaseType.Out);
		tween.Start();
		await ToSignal(tween, "tween_all_completed");
		this.Visible = false;
	}
}
