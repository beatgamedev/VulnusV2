using Godot;
using System;

public class RenderSettings : Control
{
	public override void _Ready()
	{
		var usedDriver = OS.GetCurrentVideoDriver();
		var projDriver = (string)ProjectSettings.GetSetting("rendering/quality/driver/driver_name") == "GLES2" ? OS.VideoDriver.Gles2 : OS.VideoDriver.Gles3;
		GetNode<CheckButton>("GLES2").Disabled = (usedDriver != projDriver) || !OS.HasFeature("pc");
		GetNode<CheckButton>("GLES2").Pressed = (usedDriver == OS.VideoDriver.Gles2) || (projDriver == OS.VideoDriver.Gles2);
		GetNode<CheckButton>("GLES2").Connect("pressed", this, nameof(OnButtonPressed));
	}
	public void OnButtonPressed()
	{
		if (GetNode<CheckButton>("GLES2").Pressed)
			ProjectSettings.SetSetting("rendering/quality/driver/driver_name", "GLES2");
		else
			ProjectSettings.SetSetting("rendering/quality/driver/driver_name", "GLES3");
		var newDriver = GetNode<CheckButton>("GLES2").Pressed ? OS.VideoDriver.Gles2 : OS.VideoDriver.Gles3;
		if (OS.GetCurrentVideoDriver() != newDriver)
		{
			ProjectSettings.SaveCustom("override.cfg");
			GetTree().Quit();
		}
	}
}
