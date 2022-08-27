using Godot;
using System;

public class Game : Spatial
{
	public static Map LoadedMap;
	public static Map.MapData LoadedMapData;

	public GameCamera Camera;
	public Spatial Cursor;
	public Spatial GhostCursor;

	public NoteManager NoteManager;
	public SyncManager SyncManager;

	public override void _Ready()
	{
		Camera = GetNode<GameCamera>("Camera");
		Cursor = GetNode<Spatial>("Cursor");
		GhostCursor = GetNode<Spatial>("GhostCursor");

		NoteManager = GetNode<NoteManager>("NoteManager");
		SyncManager = GetNode<SyncManager>("SyncManager");

		Camera.Cursor = Cursor;
		Camera.GhostCursor = GhostCursor;

		if (LoadedMap == null || LoadedMapData == null)
		{
			Global.Instance.GotoScene("res://scenes/MainMenu.tscn");
			return;
		}

		SyncManager.SetStream(LoadedMap.LoadAudio());
	}
}
