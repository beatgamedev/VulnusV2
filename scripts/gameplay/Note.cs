using Godot;
using System;

public class Note : Reference
{
	public static float AABB = (1.75f + 0.525f) / 2f;
	public float X;
	public float Y;
	public float T;
	public Color Color;
	public uint Index;
	public bool Hit;
	public Note(float x, float y, float t, uint i)
	{
		X = x;
		Y = y;
		T = t;
		Index = i;
		Color = new Color(i % 2 == 0 ? "#ff0000" : "#00ffff");
		Hit = false;
	}
	public bool CalculateVisibility(float noteTime, float approachTime, float hitWindow)
	{
		if (Hit)
			return false;
		return CalculateTime(noteTime, approachTime) <= 1 && (noteTime - T) <= hitWindow;
	}
	public float CalculateTime(float noteTime, float approachTime)
	{
		return (T - noteTime) / approachTime;
	}
	public bool IsTouching(Vector2 cursorPosition)
	{
		return Mathf.Abs(cursorPosition.x - X) <= AABB && Mathf.Abs(cursorPosition.y - Y) <= AABB;
	}
}
