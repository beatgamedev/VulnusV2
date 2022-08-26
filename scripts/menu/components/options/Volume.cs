using Godot;
using System;

public class Volume : Control
{
	public override void _Ready()
	{
		Slider master = GetNode<Slider>("Master");
		master.SetValue(Settings.Volume[0]);
		master.Connect("ValueChanged", this, nameof(OnValueChanged), new Godot.Collections.Array(0));
		Slider music = GetNode<Slider>("Music");
		music.SetValue(Settings.Volume[1]);
		music.Connect("ValueChanged", this, nameof(OnValueChanged), new Godot.Collections.Array(1));
		Slider sfx = GetNode<Slider>("SFX");
		sfx.SetValue(Settings.Volume[2]);
		sfx.Connect("ValueChanged", this, nameof(OnValueChanged), new Godot.Collections.Array(2));
	}
	public void OnValueChanged(float value, int index)
	{
		Settings.Volume[index] = (int)value;
		Settings.UpdateSettings(false);
	}
}