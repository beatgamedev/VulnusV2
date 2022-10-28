using Godot;
using System;

public class MapsetButton : Button
{
	public bool Selected = false;
	public BeatmapSet Mapset;
	public void ManualUpdate()
	{
		if (Mapset == null)
			return;
		GetNode<TextureRect>("Cover").Texture = Mapset.LoadCover();
		GetNode<Label>("Title").Text = Mapset.Title;
		GetNode<Label>("Title/Artist").Text = Mapset.Artist;
	}
}
