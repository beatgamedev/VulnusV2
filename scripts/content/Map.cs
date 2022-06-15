using Godot;
using System;
using System.Collections.Generic;

[Serializable]
public class Map
{
	public static int LatestFormat = 2;
	public int FormatVersion;
	public string Artist;
	public string Title;
	public List<string> DifficultyPaths;
	[NonSerialized]
	public List<Difficulty> Difficulties;
	public List<string> Mappers;
	public string Music;
	public struct Note {
		public float X;
		public float Y;
		public float T;
	}
	public struct Difficulty {
		public string Name;
		public List<Note> Notes;
	}
}
