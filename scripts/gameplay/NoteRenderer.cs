using Godot;
using System;

public class NoteRenderer : MultiMeshInstance
{
	public NoteManager NoteManager;

	public override void _Ready()
	{
		NoteManager = GetParent<NoteManager>();
	}
}
