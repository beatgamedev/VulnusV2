using Godot;
using System;

public class GameCamera : Godot.Camera
{
	public float Yaw = 0f;
	public float Pitch = 0f;
	public Vector2 CursorPosition = new Vector2();
	public override void _EnterTree()
	{
		Yaw = 0f;
		Pitch = 0f;
		CursorPosition = new Vector2();
		Input.SetMouseMode(Input.MouseMode.Captured);
	}
	public override void _ExitTree()
	{
		Input.SetMouseMode(Input.MouseMode.Visible);
	}
	public override void _Process(float delta)
	{
		Rotation = new Vector3(Pitch, Yaw, 0);
		Translation = (new Vector3(CursorPosition.x, CursorPosition.y, 0) / 4f) - (Transform.basis.z / 2f);
	}
	public override void _Input(InputEvent @event)
	{
		var input = @event as InputEventMouseMotion;
		var relative = input.Relative * Settings.MouseSensitivity;
		if (Settings.CameraMode == 0)
		{
			Yaw = Mathf.Wrap(Yaw + relative.x, -180f, 180f);
			Pitch = Mathf.Wrap(Pitch + relative.y, -90f, 90f);
		}
		else
		{
			CursorPosition += new Vector2(relative.x, relative.y);
		}
	}
}
