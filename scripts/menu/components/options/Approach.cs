using Godot;
using System;

public class Approach : Control
{
	public override void _Ready()
	{
		UpdateSettings();
		GetNode<Dropdown>("Method").Connect("ValueChanged", this, nameof(OnDropdownChanged));
		GetNode<DecimalInput>("Distance").Connect("ValueChanged", this, nameof(OnValueChanged), new Godot.Collections.Array(0));
		GetNode<DecimalInput>("Time").Connect("ValueChanged", this, nameof(OnValueChanged), new Godot.Collections.Array(1));
		GetNode<DecimalInput>("Speed").Connect("ValueChanged", this, nameof(OnValueChanged), new Godot.Collections.Array(2));
	}
	public void UpdateSettings()
	{
		GetNode<Dropdown>("Method").SetValue(Settings.ApproachMode);
		GetNode<DecimalInput>("Distance").SetValue(Settings.ApproachDistance);
		GetNode<DecimalInput>("Time").SetValue(Settings.ApproachTime);
		GetNode<DecimalInput>("Speed").SetValue(Settings.ApproachRate);
		switch (Settings.ApproachMode)
		{
			case 0:
				GetNode<SpinBox>("Distance/SpinBox").Editable = true;
				GetNode<SpinBox>("Time/SpinBox").Editable = true;
				GetNode<SpinBox>("Speed/SpinBox").Editable = false;
				break;
			case 1:
				GetNode<SpinBox>("Distance/SpinBox").Editable = true;
				GetNode<SpinBox>("Time/SpinBox").Editable = false;
				GetNode<SpinBox>("Speed/SpinBox").Editable = true;
				break;
			case 2:
				GetNode<SpinBox>("Distance/SpinBox").Editable = false;
				GetNode<SpinBox>("Time/SpinBox").Editable = true;
				GetNode<SpinBox>("Speed/SpinBox").Editable = true;
				break;
			default:
				break;
		}
	}
	public void OnValueChanged(float value, int spinbox)
	{
		switch (spinbox)
		{
			case 0:
				Settings.ApproachDistance = value;
				break;
			case 1:
				Settings.ApproachTime = value;
				break;
			case 2:
				Settings.ApproachRate = value;
				break;
			default:
				break;
		}
		Settings.UpdateSettings();
		UpdateSettings();
	}
	public void OnDropdownChanged(int index)
	{
		Settings.ApproachMode = index;
		Settings.UpdateSettings();
		UpdateSettings();
	}
}
