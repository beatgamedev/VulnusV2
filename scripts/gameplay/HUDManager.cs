using Godot;
using System;

public class HUDManager : Node
{
	public LeftPanel LeftPanel;
	public RightPanel RightPanel;

	public override void _Ready()
	{
		LeftPanel = GetNode<LeftPanel>("LeftHUD/UI");
		RightPanel = GetNode<RightPanel>("RightHUD/UI");
		RightPanel.UpdateMultiplier(1, 0);
		RightPanel.UpdateCombo(0, 0);
		// RightPanel.UpdateAccuracy(0, 1);
	}
	public void ManualUpdate(Score score)
	{
		RightPanel.UpdateMultiplier(score.Multiplier, score.Miniplier);
		RightPanel.UpdateCombo(score.Combo, score.Misses);
		// RightPanel.UpdateAccuracy(score.Misses, score.Total);
	}
}
