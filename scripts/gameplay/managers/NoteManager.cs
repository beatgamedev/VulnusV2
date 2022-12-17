using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public class NoteManager : Node
{
	internal static float HitWindow = 1.75f / 30f;

	public Game Game;
	public GameCamera Camera;

	public List<Note> Notes;
	public List<Note> OrderedNotes;
	public NoteRenderer NoteRenderer;
	public SyncManager SyncManager;

	public Note NextNote;
	public Note LastNote;

	public float ApproachTime;

	public override void _Ready()
	{
		Game = GetParent<Game>();
		Camera = Game.GetNode<GameCamera>("Camera");
		NoteRenderer = GetNode<NoteRenderer>("NoteRenderer");
		SyncManager = Game.GetNode<SyncManager>("SyncManager");
		Notes = new List<Note>();
		var sorted = Game.LoadedMapData.Notes.OrderBy(note => note.T).ToList();
		int mapIndex = 0;
		foreach (NoteData noteData in Game.LoadedMapData.Notes)
		{
			var i = sorted.IndexOf(noteData);
			var note = new Note(noteData.X * -2, noteData.Y * 2, noteData.T, i);
			note.Data = noteData;
			note.Color = new Color(mapIndex % 2 == 0 ? "#ff0000" : "#00ffff");
			Notes.Add(note);
			mapIndex++;
		}
		OrderedNotes = Notes.OrderBy(note => note.T).ToList();
		if (Notes.Count > 0)
			NextNote = Notes[0];
		ApproachTime = Settings.ApproachTime;
	}
	public override void _Process(float delta)
	{
		ApproachTime = Settings.ApproachTime * SyncManager.Speed;
		var visibleNotes = Notes.FindAll(note => note.CalculateVisibility(SyncManager.NoteTime, ApproachTime));
		visibleNotes.TrimExcess();
		visibleNotes.Reverse();
		NoteRenderer.SetNotes(visibleNotes.ToArray());
	}
	public override void _PhysicsProcess(float delta)
	{
		var visibleNotes = Notes.FindAll(note => note.CalculateTime(SyncManager.NoteTime, ApproachTime) <= 0f && !note.Hit);
		foreach (Note note in visibleNotes)
		{
			bool didHitreg = false;
			if (note.IsTouching(Camera.ClampedCursorPosition))
			{
				EmitSignal(nameof(NoteHit), note);
				didHitreg = true;
				note.Hit = true;
			}
			if (!note.Hit && !note.InHitWindow(SyncManager.NoteTime, true))
			{
				EmitSignal(nameof(NoteMiss), note);
				didHitreg = true;
				note.Hit = true;
			}
			if (didHitreg)
			{
				LastNote = note;
				if (note.Index < Notes.Count - 1 && (NextNote == null || note.Index >= NextNote.Index))
					NextNote = OrderedNotes[(int)note.Index + 1];
				else if (note.Index >= Notes.Count - 1)
					NextNote = null;
			}
			continue;
		}
	}
	[Signal]
	public delegate void NoteHit(Note note);
	[Signal]
	public delegate void NoteMiss(Note note);
}
