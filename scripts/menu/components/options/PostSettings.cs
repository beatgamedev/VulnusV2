using Godot;
using System;

public class PostSettings : Control
{
	public override void _Ready()
	{
		GetNode<Dropdown>("Bloom").SetValue(Settings.Bloom);
		GetNode<Dropdown>("Bloom").Connect("ValueChanged", this, nameof(OnDropdownChanged));
	}
	public void OnDropdownChanged(int index)
	{
		Settings.Bloom = index;
		Settings.UpdateSettings();
	}
}
