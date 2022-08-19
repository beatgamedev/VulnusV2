using Godot;
using System;

public class MapDetails : View
{
	private Map currentMap;
	private Map.Difficulty currentDifficulty;
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
		var map = MapLoader.LoadedMaps.Find(m => m.Hash == hash);
		currentMap = map;
		SetActive(true);
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
	}
	public void OnDifficultySelected(int idx)
	{
		var diff = currentMap.Difficulties[idx];
		if (diff == currentDifficulty) return;
		diff.Load(currentMap);
		currentDifficulty = diff;
	}
}