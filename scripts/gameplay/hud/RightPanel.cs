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
		tween.RemoveAll();
		tween.InterpolateProperty(GetNode<TextureProgress>("Multiplier"), "value", Miniplier, miniplier, 0.1f);
		GetNode<Label>("Multiplier/Value").Text = multiplier.ToString();
		Multiplier = multiplier;
		Miniplier = miniplier;
		tween.Start();
	}
	public void UpdateCombo(int combo, int misses)
	{
		GetNode<Label>("Combo").Text = String.Format("{0:n0}", combo);
		GetNode<Label>("Misses").Text = String.Format("{0:n0}", misses);
	}
}
