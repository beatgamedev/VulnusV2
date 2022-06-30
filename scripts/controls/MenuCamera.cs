using Godot;
using System;

public class MenuCamera : Camera
{
	public float Pitch = 0;
	public float Yaw = 0;
	public override void _Process(float delta)
	{
		LookAt(this.Translation + Vector3.Forward, this.Translation + Vector3.Up);
		RotateX(Pitch);
		RotateY(Yaw);
	}
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion)
		{
			var mouse = @event as InputEventMouseMotion;
			Pitch += mouse.Relative.y * 0.1f;
			Yaw += mouse.Relative.x * 0.1f;
			Pitch = Mathf.Clamp(Pitch, -89, 89);
		}
	}
}
