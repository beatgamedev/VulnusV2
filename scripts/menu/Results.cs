using Godot;
using System;

public class Results : View
{
	public override void _Ready()
	{
		base._Ready();
		if (Game.Score == null)
			return;
		var info = GetNode<Control>("Info");
		var mapInfo = info.GetNode<Control>("Map");
		mapInfo.GetNode<TextureRect>("Cover").Texture = Game.LoadedMapset.LoadCover();
		mapInfo.GetNode<Label>("Title").Text = Game.LoadedMapset.Name;
		mapInfo.GetNode<Label>("Mappers").Text = Game.LoadedMapset.Mappers;
		mapInfo.GetNode<Label>("Difficulty").Text = Game.LoadedMap.Name;
		info.GetNode<Label>("Score").Text = String.Format("{0:n0}", Game.Score.Points);
		info.GetNode<Label>("Combo").Text = String.Format("{0:n0}", Game.Score.HighestCombo);
		info.GetNode<Label>("Hits").Text = String.Format("{0:n0}", Game.Score.Total - Game.Score.Misses);
		info.GetNode<Label>("Misses").Text = String.Format("{0:n0}", Game.Score.Misses);
		double accuracy = Game.Score.Total > 0 ? (double)(Game.Score.Total - Game.Score.Misses) / (double)Game.Score.Total : 0;
		info.GetNode<Label>("Accuracy").Text = accuracy > 0 ? String.Format("{0:.##}%", accuracy * 100) : "0%";
		info.GetNode<Label>("Rank").Text = Score.GetRankForAccuracy(accuracy);

		GetNode<Button>("Retry").Connect("pressed", Global.Instance, nameof(Global.GotoScene), new Godot.Collections.Array("res://scenes/Game.tscn", null));
		var menuHandler = GetParent().GetParent<MenuHandler>();
		GetNode<Button>("Return").Connect("pressed", menuHandler, nameof(MenuHandler.GoTo), new Godot.Collections.Array(1));
	}
}
