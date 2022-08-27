using Godot;
using System;

public class GameCamera : Godot.Camera
{
	public float Yaw = 0f;
	public float Pitch = 0f;
	public Vector2 CursorPosition = new Vector2();
	public Vector2 ClampedCursorPosition = new Vector2();
	public Spatial Cursor = null;
	public Spatial GhostCursor = null;
	private static float clamp = (6f - 0.525f) / 2f;
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
	public override void _Input(InputEvent @event)
	{
		if (!(@event is InputEventMouseMotion))
			return;
		var input = @event as InputEventMouseMotion;
		Rotation = new Vector3(Mathf.Deg2Rad(Pitch), Mathf.Deg2Rad(Yaw), 0);
		Translation = new Vector3(0, 0, 7) + new Vector3(ClampedCursorPosition.x, ClampedCursorPosition.y, 0) / 4f - Transform.basis.z / 2f;
		var relative = input.Relative * Settings.MouseSensitivity;
		if (Settings.CameraMode == 0)
		{
			Yaw = Mathf.Wrap(Yaw - relative.x, -180f, 180f);
			Pitch = Mathf.Clamp(Pitch - relative.y, -90f, 90f);
			var position = new Vector2(Translation.x, Translation.y);
			var look = Transform.basis.z;
			CursorPosition = position + new Vector2(look.x, look.y) * -Mathf.Abs(Translation.z) / look.z;
			// v74 = v70.Position + Vector3.new(-math.abs(v70.LookVector.X), v70.LookVector.Y, v70.LookVector.Z) * math.abs(math.abs(v70.Position.X - l__Screen__49.Position.X + v22.Size.X / 2) / v70.LookVector.X);
		}
		else
		{
			Yaw = 0f;
			Pitch = 0f;
			CursorPosition += new Vector2(relative.x, -relative.y) * 0.065f;
		}
		ClampedCursorPosition = new Vector2(Mathf.Clamp(CursorPosition.x, -clamp, clamp), Mathf.Clamp(CursorPosition.y, -clamp, clamp));
		if (Cursor != null)
			Cursor.Translation = new Vector3(ClampedCursorPosition.x, ClampedCursorPosition.y, 0);
		if (GhostCursor != null)
			GhostCursor.Translation = new Vector3(CursorPosition.x, CursorPosition.y, 0);
	}
}
