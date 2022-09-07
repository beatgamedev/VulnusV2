using Godot;
using System;
using System.Collections.Generic;

public class LeftPanel : Panel
{
	public void UpdateScore(int score)
	{
		GetNode<Label>("Score").Text = String.Format("{0:n0}", score);
	}
	public void UpdateAccuracy(double misses, double total)
	{
		if (total < 1)
			total = 1;
		double accuracy = (total - misses) / total;
		GetNode<Label>("Accuracy").Text = String.Format("{0:.##}%", accuracy * 100);
		GetNode<Label>("Rank").Text = Score.GetRankForAccuracy(accuracy);
	}
}
