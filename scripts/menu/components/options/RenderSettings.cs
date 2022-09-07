using Godot;
using System;

public class RenderSettings : Control
{
	public override void _Ready()
	{
		var usedDriver = OS.GetCurrentVideoDriver();
		var projDriver = (string)ProjectSettings.GetSetting("rendering/quality/driver/driver_name") == "GLES2" ? OS.VideoDriver.Gles2 : OS.VideoDriver.Gles3;
		GetNode<CheckButton>("VSync").Disabled = (usedDriver != projDriver) || !OS.HasFeature("pc");
		GetNode<CheckButton>("VSync").Pressed = (usedDriver == OS.VideoDriver.Gles2) || (projDriver == OS.VideoDriver.Gles2);
		GetNode<CheckButton>("VSync").Connect("pressed", this, nameof(OnButtonPressed), new Godot.Collections.Array(0));
		GetNode<DecimalInput>("FPS").SetValue(Settings.FPSLimit);
		GetNode<DecimalInput>("FPS").Connect("ValueChanged", this, nameof(OnValueChanged));
	}
	public void OnValueChanged(float value)
	{
		Settings.FPSLimit = Mathf.RoundToInt(value);
		Settings.UpdateSettings();
	}
	public void OnButtonPressed(int btn)
	{
		if (btn == 0)
			Settings.VSync = GetNode<CheckButton>("VSync").Pressed;
		else
			Settings.Debanding = GetNode<CheckButton>("Debanding").Pressed;
		Settings.UpdateSettings();
	}
}
