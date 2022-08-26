using Godot;
using System;

public class DecimalInput : Control
{
	public float Value = 0;
	public override void _Ready()
	{
		GetNode<SpinBox>("SpinBox").Connect("value_changed", this, nameof(OnValueChanged));
	}
	public void SetValue(float value)
	{
		Value = value;
		GetNode<SpinBox>("SpinBox").Value = value;
	}
	public void OnValueChanged(int value)
	{
		Value = value;
		EmitSignal(nameof(ValueChanged), value);
	}
	[Signal]
	public delegate void ValueChanged(int value);
}
