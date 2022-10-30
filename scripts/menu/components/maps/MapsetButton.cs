using Godot;
using System;

public class MapsetButton : Button
{
	public BeatmapSet Mapset;
	public void ManualUpdate(bool resetButtons = false)
	{
		if (Mapset == null)
			return;
		GetNode<TextureRect>("Cover").Texture = Mapset.LoadCover();
		GetNode<Label>("Title").Text = Mapset.Title;
		GetNode<Label>("Title/Artist").Text = Mapset.Artist;
		GetNode<Label>("Title/Mapper").Text = Mapset.Mappers;
		if (resetButtons)
			ResetButtons();
	}
	private void ResetButtons()
	{

	}
}
