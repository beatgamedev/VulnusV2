using Godot;
using System;

public class Note : Reference
{
	internal static float HitWindow = 1.75f / 30f;

	public static float AABB = (1.75f + 0.525f) / 2f;
	public float X;
	public float Y;
	public float T;
	public Color Color;
	public NoteData Data;
	public int Index;
	public bool Hit;
	public Note(float x, float y, float t, int i)
	{
		X = x;
		Y = y;
		T = t;
		Index = i;
		Hit = false;
	}
	public bool CalculateVisibility(double noteTime, double approachTime)
	{
		if (Hit)
			return false;
		return CalculateTime(noteTime, approachTime) <= 1 && InHitWindow(noteTime, true);
	}
	public double CalculateTime(double noteTime, double approachTime)
	{
		return (T - noteTime) / approachTime;
	}
	public bool InHitWindow(double noteTime, bool inclusive)
	{
		if (inclusive)
			return (noteTime - T) <= HitWindow;
		return (noteTime - T) >= 0 && (noteTime - T) <= HitWindow;
	}
	public bool IsTouching(Vector2 cursorPosition)
	{
		return Mathf.Abs(cursorPosition.x - X) <= AABB && Mathf.Abs(cursorPosition.y - Y) <= AABB;
	}
}
