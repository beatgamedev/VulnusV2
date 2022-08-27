using Godot;
using System;

public class Game : Spatial
{
	public static Map LoadedMap;
	public static Map.MapData LoadedMapData;

	public GameCamera Camera;
	public Spatial Cursor;
	public Spatial GhostCursor;

	public AudioStreamPlayer MusicPlayer;

	public NoteManager NoteManager;
	public MultiMeshInstance NoteRenderer;

	public override void _Ready()
	{
		Camera = GetNode<GameCamera>("Camera");
		Cursor = GetNode<Spatial>("Cursor");
		GhostCursor = GetNode<Spatial>("GhostCursor");
		MusicPlayer = GetNode<AudioStreamPlayer>("Music");

		NoteManager = GetNode<NoteManager>("NoteManager");

		Camera.Cursor = Cursor;
		Camera.GhostCursor = GhostCursor;

		if (LoadedMap == null || LoadedMapData == null)
		{
			Global.Instance.GotoScene("res://scenes/MainMenu.tscn");
			return;
		}

		MusicPlayer.Stream = LoadedMap.LoadAudio();
	}
}
