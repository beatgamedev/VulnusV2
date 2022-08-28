using Godot;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

[Serializable, JsonObject(MemberSerialization.OptIn)]
public class Beatmap
{
	[JsonProperty("_name")]
	public string Name;
	public string Path;
	[NonSerialized]
	public BeatmapData Data;
	public void Load(BeatmapSet map)
	{
		if (Data != null) return;
		var file = new File();
		GD.Print(map.Hash, " ", file.Open(map.Path.PlusFile(Path), File.ModeFlags.Read));
		var data = JsonConvert.DeserializeObject<BeatmapData>(file.GetAsText());
		this.Data = data;
		file.Close();
	}
}
public class BeatmapData : Beatmap
{
	[JsonProperty("_notes")]
	public List<NoteData> Notes;
}
public class NoteData
{
	[JsonProperty("_x")]
	public float X;
	[JsonProperty("_y")]
	public float Y;
	[JsonProperty("_time")]
	public float T;
}