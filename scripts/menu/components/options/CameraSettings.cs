using Godot;
using System;

public class CameraSettings : Control
{
	public override void _Ready()
	{
		UpdateSettings();
		GetNode<Dropdown>("Mode").Connect("OnValueChanged", this, nameof(OnDropdownChanged));
		GetNode<DecimalInput>("Sensitivity").Connect("OnValueChanged", this, nameof(OnValueChanged));
	}
	public void UpdateSettings()
	{
		GetNode<Dropdown>("Mode").SetValue(Settings.CameraMode);
		GetNode<DecimalInput>("Sensitivity").SetValue(Settings.MouseSensitivity);
	}
	public void OnValueChanged(float value, int spinbox)
	{
		Settings.MouseSensitivity = value;
		Settings.UpdateSettings();
	}
	public void OnDropdownChanged(int index, int dropdown)
	{
		Settings.CameraMode = index;
		Settings.UpdateSettings();
	}
}
