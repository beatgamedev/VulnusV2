using Godot;
using System;

public class NoteManager : Node
{
	public Game Game;

	public NoteRenderer NoteRenderer;

	public override void _Ready()
	{
		Game = GetParent<Game>();
		NoteRenderer = GetNode<NoteRenderer>("NoteRenderer");
	}
	public override void _Process(float delta)
	{

	}
	public override void _PhysicsProcess(float delta)
	{

	}
}
