using Godot;
using System;

public class HUDManager : Node
{
	public LeftPanel LeftPanel;
	public RightPanel RightPanel;

	public override void _Ready()
	{
		LeftPanel = GetNode<LeftPanel>("LeftHUD/UI");
		LeftPanel.UpdateAccuracy(0, 1);
		RightPanel = GetNode<RightPanel>("RightHUD/UI");
		RightPanel.UpdateMultiplier(1, 0);
		RightPanel.UpdateCombo(0, 0);
	}
	public void ManualUpdate(Score score)
	{
		LeftPanel.UpdateScore(score.Points);
		LeftPanel.UpdateAccuracy(score.Misses, score.Total);
		RightPanel.UpdateMultiplier(score.Multiplier, score.Miniplier);
		RightPanel.UpdateCombo(score.Combo, score.Misses);
	}
}
