using Godot;
using System;

public class Note : Reference
{
	public static float AABB = (1.75f + 0.525f) / 2f;
	public float X;
	public float Y;
	public float T;
	public bool Hit;
	public Note(float x, float y, float t)
	{
		X = x;
		Y = y;
		T = t;
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
}
