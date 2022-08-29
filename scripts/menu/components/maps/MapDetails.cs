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
		base._Ready();
		SetActive(false);
		mapList = GetParent().GetNode<MapList>("MapList");
		mapList.Connect("MapSelected", this, nameof(OnMapSelected));
		GetNode<OptionButton>("Difficulty").Connect("item_selected", this, nameof(OnDifficultySelected));
		GetNode<Button>("Play").Connect("pressed", this, nameof(OnPlayPressed));
	}
	public void OnPlayPressed()
	{
		GetNode<AudioStreamPlayer>("MusicPreview").Stop();
		Global.Instance.GotoScene("res://scenes/Game.tscn");
	}
	public void OnMapSelected(string hash)
	{
		var preview = GetNode<AudioStreamPlayer>("MusicPreview");
		if (hash == null)
		{
			preview.Stop();
			SetActive(false);
			return;
		}
		if (!this.IsActive)
			SetActive(true);
		var map = BeatmapLoader.LoadedMaps.Find(m => m.Hash == hash);
		currentMap = map;
		GetNode<TextureRect>("Cover").Texture = map.LoadCover();
		GetNode<Label>("Title").Text = map.Name;
		GetNode<Label>("Mappers").Text = map.Mappers;
		GetNode<OptionButton>("Difficulty").Clear();
		var i = 0;
		foreach (var difficulty in map.Difficulties)
		{
			GetNode<OptionButton>("Difficulty").AddItem(difficulty.Name, i);
			i++;
		}
		GetNode<OptionButton>("Difficulty").Selected = 0;
		OnDifficultySelected(0);
		preview.Stream = null;
		preview.Stream = map.LoadAudio();
		preview.Play(preview.Stream.GetLength() / 3);
		Game.LoadedMapset = currentMap;
	}
	public void OnDifficultySelected(int idx)
	{
		var diff = currentMap.Difficulties[idx];
		if (diff == currentDifficulty) return;
		GetNode<Button>("Play").Visible = diff.Load(currentMap) == Error.Ok;
		currentDifficulty = diff;
		Game.LoadedMap = currentDifficulty;
		Game.LoadedMapData = currentDifficulty.Data;
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
