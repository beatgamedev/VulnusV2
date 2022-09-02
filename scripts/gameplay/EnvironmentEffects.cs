using Godot;
using System;

public class EnvironmentEffects : WorldEnvironment
{
	public override void _Ready()
	{
		Environment.GlowEnabled = Settings.Bloom != 2;
		if (!Environment.GlowEnabled)
			return;
		Environment.GlowHighQuality = Settings.Bloom == 0;
		Environment.GlowBicubicUpscale = Settings.Bloom == 0;
		Environment.GlowLevels__6 = Settings.Bloom == 0;
		Environment.GlowLevels__7 = Settings.Bloom == 0;
	}
}
