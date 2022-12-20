using Godot;
using System;

public partial class View : Control
{
	protected Tween ViewTween;
	public override void _EnterTree()
	{
		ViewTween = new Tween();
		AddChild(ViewTween);
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
		this.IsActive = true;
		this.Visible = true;
		ViewTween.RemoveAll();
		this.RectPosition = new Vector2(0, 0);
		ViewTween.InterpolateProperty(this, "modulate:a", 0, 1, 0.15f, Tween.TransitionType.Sine, Tween.EaseType.Out);
		ViewTween.Start();
		await ToSignal(ViewTween, "tween_all_completed");
	}
	public virtual async void OnHide()
	{
		this.IsActive = false;
		ViewTween.RemoveAll();
		ViewTween.InterpolateProperty(this, "rect_position:y", 0, 64, 0.15f, Tween.TransitionType.Sine, Tween.EaseType.Out);
		ViewTween.InterpolateProperty(this, "modulate:a", 1, 0, 0.15f, Tween.TransitionType.Sine, Tween.EaseType.Out);
		ViewTween.Start();
		await ToSignal(ViewTween, "tween_all_completed");
		this.Visible = false;
	}
}
