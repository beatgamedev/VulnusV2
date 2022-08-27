using Godot;
using System;
using System.Collections.Generic;

public class NoteManager : Node
{
	internal static float HitWindow = 1.75f / 30f;

	public Game Game;
	public GameCamera Camera;

	public List<Note> Notes;
	public NoteRenderer NoteRenderer;
	public SyncManager SyncManager;

	public Note NextNote;
	public Note LastNote;

	public override void _Ready()
	{
		Game = GetParent<Game>();
		Camera = Game.GetNode<GameCamera>("Camera");
		NoteRenderer = GetNode<NoteRenderer>("NoteRenderer");
		SyncManager = Game.GetNode<SyncManager>("SyncManager");
		Notes = new List<Note>();
		uint i = 0;
		foreach (Map.Note noteData in Game.LoadedMapData.Notes)
		{
			Notes.Add(new Note(noteData.X * 2, noteData.Y * 2, noteData.T, i));
			i++;
		}
		if (Notes.Count > 0)
			NextNote = Notes[0];
	}
	public override void _Process(float delta)
	{
		var approachTime = Settings.ApproachTime;
		var visibleNotes = Notes.FindAll(note => note.CalculateVisibility(SyncManager.NoteTime, approachTime));
		visibleNotes.TrimExcess();
		visibleNotes.Reverse();
		NoteRenderer.SetNotes(visibleNotes.ToArray());
	}
	public override void _PhysicsProcess(float delta)
	{
		var approachTime = Settings.ApproachTime;
		var visibleNotes = Notes.FindAll(note => note.CalculateTime(SyncManager.NoteTime, approachTime) <= 0f);
		foreach (Note note in visibleNotes)
		{
			bool didHitreg = false;
			if (note.IsTouching(Camera.ClampedCursorPosition))
			{
				EmitSignal(nameof(NoteHit), note);
				didHitreg = true;
			}
			if (!note.Hit && !note.InHitWindow(SyncManager.NoteTime, true))
			{
				EmitSignal(nameof(NoteMiss), note);
				didHitreg = true;
			}
			if (didHitreg)
			{
				note.Hit = true;
				if (Notes.Count > note.Index)
					NextNote = Notes[(int)note.Index + 1];
				else
					NextNote = null;
				LastNote = note;
			}
			continue;
		}
	}
	[Signal]
	public delegate void NoteHit(Note note);
	[Signal]
	public delegate void NoteMiss(Note note);
}
