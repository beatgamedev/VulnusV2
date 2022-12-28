using Godot;
using System;
using System.Collections.Generic;

namespace Gameplay.Hud
{
	public class LeftPanel : Panel
	{
		private Tween tween;
		private bool displayingSkip = false;
		public override void _Ready()
		{
			tween = GetNode<Tween>("Tween");
		}
		public void UpdateScore(int score)
		{
			GetNode<Label>("Score").Text = String.Format("{0:n0}", score);
		}
		public void UpdateAccuracy(double misses, double total)
		{
			if (total < 1)
				total = 1;
			double accuracy = total > 0 ? (total - misses) / total : 0;
			GetNode<Label>("Accuracy").Text = String.Format("{0:.##}%", accuracy * 100);
			GetNode<Label>("Rank").Text = Score.GetRankForAccuracy(accuracy);
		}
		public void UpdateSkip(bool skippable)
		{
			if (skippable && !displayingSkip)
			{
				displayingSkip = true;
				tween.RemoveAll();
				tween.InterpolateProperty(GetNode<Label>("Skippable"), "modulate:a", 0, 1, 2f, Tween.TransitionType.Sine);
				tween.Start();
			}
			else if (displayingSkip && !skippable)
			{
				displayingSkip = false;
				var a = GetNode<Label>("Skippable").Modulate.a;
				tween.RemoveAll();
				tween.InterpolateProperty(GetNode<Label>("Skippable"), "modulate:a", a, 0, 0.5f * a, Tween.TransitionType.Sine);
				tween.Start();
			}
		}
	}
}
