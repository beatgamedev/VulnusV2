using Godot;
using System;

public class Game : Spatial
{
	public static Map LoadedMap;
	public static Map.MapData LoadedMapData;

	public static Score Score;

	public GameCamera Camera;
	public Spatial Cursor;
	public Spatial GhostCursor;

	public NoteManager NoteManager;
	public SyncManager SyncManager;

	public bool Ended;

	public override void _Ready()
	{
		Camera = GetNode<GameCamera>("Camera");
		Cursor = GetNode<Spatial>("Cursor");
		GhostCursor = GetNode<Spatial>("GhostCursor");

		NoteManager = GetNode<NoteManager>("NoteManager");
		SyncManager = GetNode<SyncManager>("SyncManager");

		Score = new Score();

		Ended = false;

		Camera.Cursor = Cursor;
		Camera.GhostCursor = GhostCursor;

		if (LoadedMap == null || LoadedMapData == null)
		{
			Global.Instance.GotoScene("res://scenes/MainMenu.tscn");
			return;
		}

		SyncManager.SetStream(LoadedMap.LoadAudio());
		SyncManager.Connect("Ended", this, nameof(GameEnded));
	}
	public void GameEnded()
	{
		Ended = true;
		Global.Instance.GotoScene("res://scenes/MainMenu.tscn");
	}
}
