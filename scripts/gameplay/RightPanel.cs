using Godot;
using System;

public class RightPanel : Panel
{
	private Tween tween;
	private int Miniplier;
	private int Multiplier;
	public override void _Ready()
	{
		tween = GetNode<Tween>("Tween");
	}
	public void UpdateMultiplier(int multiplier, int miniplier)
	{
		if (multiplier == Multiplier && miniplier == Miniplier)
			return;
		tween.StopAll();
		tween.InterpolateProperty(GetNode<TextureProgress>("Multiplier"), "value", Miniplier, miniplier, 0.1f);
		GetNode<Label>("Multiplier/Value").Text = multiplier.ToString();
		Multiplier = multiplier;
		Miniplier = miniplier;
		tween.Start();
	}
	public void UpdateAccuracy(double misses, double total)
	{
		if (total < 1)
			total = 1;
		double accuracy = (total - misses) / total;
		GetNode<Label>("Accuracy").Text = String.Format("{0:.##}%", accuracy * 100);
		GetNode<Label>("Misses").Text = String.Format("{0:n0}", misses);
	}
}
