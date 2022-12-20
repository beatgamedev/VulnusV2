using Godot;
using System;
using System.Threading.Tasks;

public class MapDetails : View
{
	private BeatmapSet currentMap;
	private Beatmap currentDifficulty;
	private Task loadingMap;
	private MapList mapList;
	private Control mapDetails;
	private Control details;
	private Control loading;
	private AudioStreamPlayer musicPreview;
	public override void _Ready()
	{
		mapList = GetParent().GetNode<MapList>("MapList");
		mapList.MapSelected += MapSelected;
		details = GetNode<Control>("Details");
		mapDetails = details.GetNode<Control>("AspectRatioContainer/Map");
		loading = GetNode<Control>("Loading");
		musicPreview = GetNode<AudioStreamPlayer>("MusicPreview");

		details.GetNode<Button>("Play").Connect("pressed", this, nameof(PlayMap));

		SetActive(false);
	}
	public void PlayMap()
	{
		if (!currentDifficulty.Playable)
			return;
		Game.LoadedMapset = currentMap;
		Game.LoadedMap = currentDifficulty;
		Game.LoadedMapData = currentDifficulty.Data;
		Game.DebugSpeed = (float)details.GetNode<SpinBox>("Speed").Value;
		Global.Instance.GotoScene("res://scenes/Game.tscn", (Node scn) =>
		{
			var menuHandler = GetParent().GetParent<MenuHandler>();
			menuHandler.GoTo(2);
		});
	}
	public void MapSelected(Beatmap map)
	{
		currentMap = map.Mapset;
		currentDifficulty = map;
		if (!this.IsActive)
			SetActive(true);
		mapDetails.GetNode<TextureRect>("Cover").Texture = currentMap.LoadCover();
		mapDetails.GetNode<Label>("Title").Text = currentMap.Title;
		mapDetails.GetNode<Label>("Title/Artist").Text = currentMap.Artist;
		mapDetails.GetNode<Label>("Title/Mapper").Text = currentMap.Mappers;
		mapDetails.GetNode<Label>("Difficulty").Text = map.Name;
		musicPreview.Stream = currentMap.LoadAudio();
		musicPreview.Play(musicPreview.Stream.GetLength() / 3f);
		loadingMap = Task.Run(loadMap);
	}
	private async void loadMap()
	{
		var map = currentDifficulty;
		Error loaded = map.Load();
		await Task.Delay(TimeSpan.FromSeconds(1));
		if (!map.Playable || loaded != Error.Ok)
			SetActive(false);
	}
	private float circleSpin = 0f;
	public override void _Process(float delta)
	{
		if (currentDifficulty == null || loadingMap == null)
			return;
		if (!loadingMap.IsCompleted)
		{
			circleSpin += delta;
			loading.GetNode<Control>("Circle").RectRotation = Mathf.Wrap(circleSpin * 90f, 0, 360);
			loading.Visible = true;
			details.Visible = false;
			return;
		}
		loading.Visible = false;
		details.Visible = true;
	}
	public override async void OnShow()
	{
		this.Visible = true;
		ViewTween.RemoveAll();
		ViewTween.InterpolateProperty(this, "modulate:a", 0, 1, 0.15f, Tween.TransitionType.Sine, Tween.EaseType.Out);
		ViewTween.Start();
		await ToSignal(ViewTween, "tween_all_completed");
		this.IsActive = true;
	}
	public override async void OnHide()
	{
		this.IsActive = false;
		ViewTween.RemoveAll();
		ViewTween.InterpolateProperty(this, "modulate:a", 1, 0, 0.15f, Tween.TransitionType.Sine, Tween.EaseType.Out);
		ViewTween.Start();
		await ToSignal(ViewTween, "tween_all_completed");
		this.Visible = false;
	}
}
