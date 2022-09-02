using Godot;
using System;

public class CameraSettings : Control
{
	public override void _Ready()
	{
		UpdateSettings();
		GetNode<Dropdown>("Mode").Connect("ValueChanged", this, nameof(OnDropdownChanged));
		GetNode<DecimalInput>("Sensitivity").Connect("ValueChanged", this, nameof(OnValueChanged));
		GetNode<CheckButton>("Drift").Connect("pressed", this, nameof(OnButtonPressed));
	}
	public void UpdateSettings()
	{
		GetNode<Dropdown>("Mode").SetValue(Settings.CameraMode);
		GetNode<DecimalInput>("Sensitivity").SetValue(Settings.MouseSensitivity);
		GetNode<CheckButton>("Drift").Pressed = Settings.CursorDrift;
	}
	public void OnButtonPressed()
	{
		Settings.CursorDrift = GetNode<CheckButton>("Drift").Pressed;
		Settings.UpdateSettings();
	}
	public void OnValueChanged(float value)
	{
		Settings.MouseSensitivity = value;
		Settings.UpdateSettings();
	}
	public void OnDropdownChanged(int index)
	{
		Settings.CameraMode = index;
		Settings.UpdateSettings();
	}
}
