using Godot;
using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using File = Godot.File;

[Serializable, JsonObject(MemberSerialization.OptIn)]
public class MapVersion
{
	public static int LatestFormat = 2;
	[JsonProperty("_version")]
	public int FormatVersion;
}
[Serializable, JsonObject(MemberSerialization.OptIn)]
public class Map : MapVersion
{
	[NonSerialized]
	public string Hash;
	[NonSerialized]
	public string Path;
	[JsonProperty("_artist")]
	public string Artist;
	[JsonProperty("_title")]
	public string Title;
	public string Name
	{
		get
		{
			return $"{Title} - {Artist}";
		}
	}
	[JsonProperty("_difficulties")]
	public List<string> _difficulties;
	public List<Difficulty> Difficulties;
	[JsonProperty("_mappers")]
	public List<string> _mappers;
	public string Mappers
	{
		get
		{
			return string.Join(", ", _mappers.ToArray());
		}
	}
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
		[NonSerialized, JsonIgnore]
		public MapData Data;
		public void Load(Map map)
		{
			if (Data != null) return;
			var file = new File();
			GD.Print(file.Open(map.Path.PlusFile(Path), File.ModeFlags.Read));
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
	public static Map Load(string json)
	{
		return JsonConvert.DeserializeObject<Map>(json);
	}
	public static Map LoadFromPath(string path, string hash)
	{
		path = path.Replace("user://", OS.GetUserDataDir());
		var file = new File();
		if (file.Open(path.PlusFile("cache.bin"), File.ModeFlags.Read) == Error.Ok)
		{
			// GD.Print("Loading map from cache: " + path);
			var deserializer = new BinaryFormatter();
			var buffer = file.GetBuffer((long)file.GetLen());
			var stream = new MemoryStream(buffer);
			var cachedMap = (Map)deserializer.Deserialize(stream);
			cachedMap.Path = path;
			cachedMap.Hash = hash;
			return cachedMap;
		}
		// GD.Print("Loading map without cache: " + path);
		file.Open(path.PlusFile("meta.json"), File.ModeFlags.Read);
		var map = Map.Load(file.GetAsText());
		map.Path = path;
		map.Difficulties = new List<Difficulty>();
		foreach (string difficulty in map._difficulties)
		{
			var diffFile = new File();
			diffFile.Open(path.PlusFile(difficulty), File.ModeFlags.Read);
			var diff = JsonConvert.DeserializeObject<Difficulty>(diffFile.GetAsText());
			diff.Path = difficulty;
			map.Difficulties.Add(diff);
			diffFile.Close();
		}
		file.Close();
		var writer = new FileStream(path.PlusFile("cache.bin"), FileMode.Create);
		map.SerializeToFile(ref writer);
		writer.Dispose();
		map.Hash = hash;
		return map;
	}
	public void SerializeToFile(ref FileStream stream)
	{
		var serializer = new BinaryFormatter();
		serializer.Serialize(stream, this);
		stream.Flush();
	}
	[NonSerialized]
	public Texture Cover;
	public Texture LoadCover()
	{
		if (Cover != null) return Cover;
		ImageTexture texture;
		var cover = new Image();
		var coverPath = "none";
		foreach (string path in System.IO.Directory.GetFiles(Path))
		{
			if (path.GetFile().BaseName().ToLower() == "cover")
			{
				coverPath = path;
				break;
			}
		}
		if (coverPath == "none")
		{
			Cover = Global.Matt;
			return Global.Matt;
		}
		texture = new ImageTexture();
		cover.Load(coverPath);
		texture.CreateFromImage(cover);
		Cover = texture;
		return texture;
	}
	public AudioStream LoadAudio()
	{
		return AudioHandler.LoadAudio(Path.PlusFile(Music));
	}
}
