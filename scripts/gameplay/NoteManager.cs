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

	}
	[Signal]
	public delegate void NotesFinished();
}
