using Godot;
using System;

public class HUDManager : Node
{
	public LeftPanel LeftPanel;
	public RightPanel RightPanel;
	public Health Health;

	public override void _Ready()
	{
		LeftPanel = GetNode<LeftPanel>("LeftHUD/UI");
		LeftPanel.UpdateScore(0);
		LeftPanel.UpdateAccuracy(0, 1);
		RightPanel = GetNode<RightPanel>("RightHUD/UI");
		RightPanel.UpdateMultiplier(1, 0);
		RightPanel.UpdateCombo(0, 0);
		Health = GetNode<Health>("HealthHUD/UI");
		Health.UpdateHealth(10);
	}
	public void ManualUpdate(Score score)
	{
		LeftPanel.UpdateScore(score.Points);
		LeftPanel.UpdateAccuracy(score.Misses, score.Total);
		RightPanel.UpdateMultiplier(score.Multiplier, score.Miniplier);
		RightPanel.UpdateCombo(score.Combo, score.Misses);
		Health.UpdateHealth(score.Health);
	}
}
