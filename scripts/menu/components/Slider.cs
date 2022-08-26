using Godot;
using System;

public class Slider : Control
{
	[Export]
	public string Label = "";
	[Export]
	public string Suffix = "";
	[Export]
	public float Value = 0;
	[Export]
	public float Max = 100;
	[Export]
	public float Min = 0;
	[Export]
	public float Step = 1;
	public override void _Ready()
	{
		GetNode<Label>("Label").Text = $"{Label} ({Value}{Suffix})";
		GetNode<Godot.Slider>("Slider").Value = Value;
		GetNode<Godot.Slider>("Slider").MaxValue = Max;
		GetNode<Godot.Slider>("Slider").MinValue = Min;
		GetNode<Godot.Slider>("Slider").Step = Step;
		GetNode<Godot.Slider>("Slider").Connect("value_changed", this, nameof(OnValueChanged));
	}
	public void SetValue(float value)
	{
		GetNode<Label>("Label").Text = $"{Label} ({value})";
		Value = value;
		GetNode<Godot.Slider>("Slider").Value = value;
	}
	public void OnValueChanged(float value)
	{
		GetNode<Label>("Label").Text = $"{Label} ({value})";
		Value = value;
		EmitSignal(nameof(ValueChanged), value);
	}
	[Signal]
	public delegate void ValueChanged(float value);
}
