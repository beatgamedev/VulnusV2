using System;
using System.Buffers;
using Godot;

public struct Replay
{
	internal struct Frame
	{
		public float X;
		public float Y;
		public float SongPosition;
		public Note[] Notes;
	}
	internal struct Note
	{
		public uint Index;
		public float Time;
	}
	internal enum Mods
	{

	}
	uint PlayerID;
	string MapID;
	uint Difficulty;
	Mods ModsUsed;
	Frame[] Frames;
	public static Replay LoadFromFile(File file)
	{
		var replay = new Replay();
		if (file.GetBuffer(4) != new byte[] { 0x76, 0x75, 0x6c, 0x72 })
			throw new Exception("Not valid replay");
		uint version = file.Get8();
		replay.PlayerID = file.Get32();
		uint idLen = file.Get16();
		replay.MapID = StringExtensions.GetStringFromASCII(file.GetBuffer(idLen));
		replay.Difficulty = file.Get8();
		replay.ModsUsed = (Mods)file.Get16();
		uint noFrames = file.Get16();
		replay.Frames = new Frame[noFrames];
		for (int i = 0; i < noFrames; i++)
		{
			var frame = new Frame();
			frame.X = file.GetFloat();
			frame.Y = file.GetFloat();
			frame.SongPosition = file.GetFloat();
			uint noNotes = file.Get8();
			frame.Notes = new Note[noNotes];
			for (int n = 0; n < noNotes; n++)
			{
				var note = new Note();
				note.Index = file.Get16();
				note.Time = file.GetFloat();
				frame.Notes[n] = note;
			}
			replay.Frames[i] = frame;
		}
		return replay;
	}
}