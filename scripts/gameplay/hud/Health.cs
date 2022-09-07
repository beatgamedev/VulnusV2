using Godot;
using System;

public class Health : TextureProgress
{
	private Tween tween;
	private double HealthPoints;
	public override void _Ready()
	{
		tween = GetNode<Tween>("Tween");
	}
	public void UpdateHealth(double health)
	{
		if (health == HealthPoints)
			return;
		tween.StopAll();
		tween.InterpolateProperty(this, "value", HealthPoints, health, 0.1f);
		HealthPoints = health;
		tween.Start();
	}
}
