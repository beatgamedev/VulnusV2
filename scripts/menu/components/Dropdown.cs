using Godot;
using System;

public class Dropdown : Control
{
	public int Value = 0;
	public override void _Ready()
	{
		GetNode<OptionButton>("OptionButton").Connect("item_selected", this, nameof(OnValueChanged));
	}
	public void SetValue(int value)
	{
		Value = value;
		GetNode<OptionButton>("OptionButton").Selected = value;
	}
	public void OnValueChanged(int value)
	{
		Value = value;
		EmitSignal(nameof(ValueChanged), value);
	}
	[Signal]
	public delegate void ValueChanged(int value);
}
