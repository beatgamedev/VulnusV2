using Godot;
using System;

public class Volume : Control
{
	public override void _Ready()
	{
		Slider master = GetNode<Slider>("Master");
		master.SetValue(Settings.Volume[0]);
		master.Connect("ValueChanged", this, nameof(OnValueChanged));
		Slider music = GetNode<Slider>("Music");
		music.SetValue(Settings.Volume[1]);
		music.Connect("ValueChanged", this, nameof(OnValueChanged));
		Slider sfx = GetNode<Slider>("SFX");
		sfx.SetValue(Settings.Volume[2]);
		sfx.Connect("ValueChanged", this, nameof(OnValueChanged));
	}
	public void OnValueChanged(float value)
	{
		Slider master = GetNode<Slider>("Master");
		Settings.Volume[0] = (int)master.Value;
		Slider music = GetNode<Slider>("Music");
		Settings.Volume[1] = (int)music.Value;
		Slider sfx = GetNode<Slider>("SFX");
		Settings.Volume[2] = (int)sfx.Value;
		Settings.UpdateSettings(false);
	}
}