using Godot;
using System;
using System.Collections.Generic;

public class LeftPanel : Panel
{
	public Dictionary<double, string> Ranks = new Dictionary<double, string>();
	public override void _Ready()
	{
		Ranks.Add(100, "SS");
		Ranks.Add(95, "SS");
		Ranks.Add(90, "S");
		Ranks.Add(80, "A");
		Ranks.Add(75, "B");
		Ranks.Add(60, "C");
		Ranks.Add(50, "D");
		Ranks.Add(0, "E");
	}
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
		foreach (KeyValuePair<double, string> pair in Ranks)
		{
			if (accuracy * 100 >= pair.Key)
			{
				GetNode<Label>("Rank").Text = pair.Value;
				break;
			}
		}
	}
}
