using Godot;
using System;
using System.Threading.Tasks;

public class MapDetails : View
{
	private BeatmapSet currentMap;
	private Beatmap currentDifficulty;
	private MapList mapList;
	public override void _Ready()
	{
		if (Game.Score == null)
		{
			SetActive(false);
			return;
		}
	}
	public override async void OnShow()
	{
		this.Visible = true;
		ViewTween.StopAll();
		ViewTween.InterpolateProperty(this, "modulate:a", 0, 1, 0.15f, Tween.TransitionType.Sine, Tween.EaseType.Out);
		ViewTween.Start();
		await ToSignal(ViewTween, "tween_all_completed");
		this.IsActive = true;
	}
	public override async void OnHide()
	{
		this.IsActive = false;
		ViewTween.StopAll();
		ViewTween.InterpolateProperty(this, "modulate:a", 1, 0, 0.15f, Tween.TransitionType.Sine, Tween.EaseType.Out);
		ViewTween.Start();
		await ToSignal(ViewTween, "tween_all_completed");
		this.Visible = false;
	}
}
