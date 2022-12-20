using Godot;
using System;

public class Game : Spatial
{
	public static float DebugSpeed = 1f;

	public static BeatmapSet LoadedMapset;
	public static Beatmap LoadedMap;
	public static BeatmapData LoadedMapData;

	public static Score Score;

	public GameCamera Camera;
	public Spatial Cursor;
	public Spatial GhostCursor;

	public NoteManager NoteManager;
	public SyncManager SyncManager;

	public HUDManager HUDManager;

	public bool Ended;

	public override void _Ready()
	{
		Camera = GetNode<GameCamera>("Camera");
		Cursor = GetNode<Spatial>("Cursor");
		GhostCursor = GetNode<Spatial>("GhostCursor");

		NoteManager = GetNode<NoteManager>("NoteManager");
		SyncManager = GetNode<SyncManager>("SyncManager");
		SyncManager.Speed = DebugSpeed / 100f;

		HUDManager = GetNode<HUDManager>("HUD");

		Score = new Score();

		Ended = false;

		Camera.Cursor = Cursor;
		Camera.GhostCursor = GhostCursor;

		if (LoadedMapset == null || LoadedMapData == null)
		{
			Global.Instance.GotoScene("res://scenes/MainMenu.tscn");
			return;
		}

		SyncManager.SetStream(LoadedMapset.LoadAudio());
		SyncManager.Connect("Ended", this, nameof(GameEnded));

		NoteManager.NoteHit += OnNoteHit;
		NoteManager.NoteMiss += OnNoteMiss;

		Global.Discord.SetActivity(new Discord.ActivityW(
			state: "Playing a map",
			details: $"{LoadedMapset.Name} - {LoadedMap.Name}",
			startTimestamp: DateTime.Now,
			endTimestamp: DateTime.Now.AddSeconds(SyncManager.AudioPlayer.Stream.GetLength() * SyncManager.Speed)
		));
	}
	public override void _PhysicsProcess(float delta)
	{
		if (Input.IsActionJustPressed("skip") && SyncManager.CanSkip())
			SyncManager.AttemptSkip();
		if (Input.IsActionJustPressed("force_end"))
		{
			Score.Health = 0;
			GameEnded();
		}
	}
	public override void _EnterTree()
	{
		Options opt = (Options)Global.Instance.Overlays["Options"];
		opt.CanOpen = false;
		if (opt.IsActive)
		{
			opt.SetActive(false);
		}
	}
	public override void _ExitTree()
	{
		Options opt = (Options)Global.Instance.Overlays["Options"];
		opt.CanOpen = true;
	}
	public void OnNoteHit(Note note)
	{
		Score.Points += 25 * Score.Multiplier;
		Score.Miniplier += 1;
		if (Score.Miniplier >= 8 && Score.Multiplier < 8)
		{
			Score.Miniplier = 0;
			Score.Multiplier = Mathf.Min(8, Score.Multiplier + 1);
		}
		Score.Combo += 1;
		if (Score.Combo > Score.HighestCombo)
			Score.HighestCombo = Score.Combo;
		Score.Total += 1;
		Score.Health = Math.Min(10, Score.Health + 10.0 / 8.0);
		HUDManager.ManualUpdate(Score);
	}
	public void OnNoteMiss(Note note)
	{
		Score.Miniplier = 0;
		Score.Multiplier = Mathf.Max(1, Score.Multiplier - 1);
		Score.Combo = 0;
		Score.Misses += 1;
		Score.Total += 1;
		Score.Health = Math.Max(0, Score.Health - 2);
		if (Score.Health <= 0)
		{
			GameEnded();
			return;
		}
		HUDManager.ManualUpdate(Score);
	}
	public void GameEnded()
	{
		Ended = true;
		SyncManager.AudioPlayer.Stop();
		Global.Instance.GotoScene("res://scenes/MainMenu.tscn");
	}
}
