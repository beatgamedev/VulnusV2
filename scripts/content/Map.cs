using Godot;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

[Serializable, JsonObject(MemberSerialization.OptIn)]
public class Map
{
	public static int LatestFormat = 2;
	[JsonProperty("_version")]
	public int FormatVersion;
	[NonSerialized]
	public string Path;
	[JsonProperty("_artist")]
	public string Artist;
	[JsonProperty("_title")]
	public string Title;
	[JsonProperty("_difficulties")]
	public List<string> _difficulties;
	public List<Difficulty> Difficulties;
	[JsonProperty("_mappers")]
	public List<string> Mappers;
	[JsonProperty("_music")]
	public string Music;
	[Serializable, JsonObject]
	public class Note
	{
		public float X;
		public float Y;
		public float T;
	}
	[Serializable, JsonObject(MemberSerialization.OptIn)]
	public class Difficulty
	{
		[JsonProperty("_name")]
		public string Name;
		[JsonIgnore]
		public string Path;
		[JsonIgnore]
		public MapData Data;
		public void Load(Map map)
		{
			if (Data != null) return;
			var file = new File();
			file.Open(map.Path + "/" + Path, File.ModeFlags.Read);
			var data = JsonConvert.DeserializeObject<MapData>(file.GetAsText());
			this.Data = data;
			file.Close();
		}
	}
	[JsonObject]
	public class MapData : Difficulty
	{
		[JsonProperty("_notes")]
		public List<Note> Notes;
	}
}