using Godot;
using System;

public class NoteRenderer : MultiMeshInstance
{
	public NoteManager NoteManager;

	public Note[] Notes = new Note[0];

	public override void _Ready()
	{
		NoteManager = GetParent<NoteManager>();
		Multimesh.InstanceCount = 0;
		Multimesh.VisibleInstanceCount = 0;
		Multimesh.TransformFormat = MultiMesh.TransformFormatEnum.Transform3d;
		Multimesh.ColorFormat = MultiMesh.ColorFormatEnum.Color8bit;
		Multimesh.CustomDataFormat = MultiMesh.CustomDataFormatEnum.None;
	}
	public override void _Process(float delta)
	{
		for (int i = 0; i < Notes.Length; i++)
		{
			var note = Notes[i];
			var noteDistance = note.CalculateTime(NoteManager.NoteTime, Settings.ApproachTime) * Settings.ApproachDistance;
			Multimesh.SetInstanceTransform(i, new Transform(Basis.Identity, new Vector3(note.X, note.Y, -noteDistance)));
			Multimesh.SetInstanceColor(i, note.Color);
		}
	}
	public void ManualUpdate()
	{
		if (Notes.Length > Multimesh.InstanceCount)
		{
			Multimesh.InstanceCount = Notes.Length;
		}
		Multimesh.VisibleInstanceCount = Notes.Length;
	}
	public void SetNotes(Note[] notes)
	{
		Notes = notes;
		ManualUpdate();
	}
}
