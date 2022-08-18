using Godot;
using System;

public class MapDetails : View
{
	private MapList mapList;
	public override void _Ready()
	{
		base._Ready();
		SetActive(false);
		mapList = GetParent().GetNode<MapList>("MapList");
		mapList.Connect("MapSelected", this, nameof(OnMapSelected));
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
		SetActive(true);
		GetNode<TextureRect>("Cover").Texture = map.LoadCover();
		GetNode<Label>("Title").Text = map.Title;
		GetNode<Label>("Mappers").Text = map.Mappers;
		preview.Stream = null;
		preview.Stream = map.LoadAudio();
		preview.Play(preview.Stream.GetLength() / 3);
	}
}