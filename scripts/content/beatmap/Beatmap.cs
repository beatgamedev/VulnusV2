using Godot;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

[Serializable, JsonObject(MemberSerialization.OptIn)]
public partial class BeatmapInfo
{
	public static int LatestFormat = 1;
	[JsonProperty("_version")]
	public int FormatVersion;
	[NonSerialized]
	public string Path;
	[NonSerialized]
	public bool Playable;
}
[Serializable, JsonObject(MemberSerialization.OptIn)]
public class Beatmap : BeatmapInfo
{
	[JsonProperty("_name")]
	public string Name;
	[NonSerialized]
	public BeatmapData Data;
	public Error Load(BeatmapSet map)
	{
		if (Data != null) return Error.Ok;
		var file = new File();
		var err = file.Open(map.Path.PlusFile(Path), File.ModeFlags.Read);
		GD.Print(map.Hash, " ", err);
		var json = file.GetAsText();
		var version = JsonConvert.DeserializeObject<BeatmapInfo>(json);
		BeatmapData data;
		switch (FormatVersion)
		{
			default:
				data = JsonConvert.DeserializeObject<BeatmapData>(json);
				break;
		}
		this.Playable = FormatVersion <= BeatmapInfo.LatestFormat;
		this.Data = data;
		file.Close();
		return err;
	}
}
public class BeatmapData
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