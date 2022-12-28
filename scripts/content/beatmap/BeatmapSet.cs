using Godot;
using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using File = Godot.File;

namespace Content.Beatmaps
{
	[Serializable, JsonObject(MemberSerialization.OptIn)]
	public partial class BeatmapSetInfo
	{
		public static int LatestFormat = 1;
		[JsonProperty("_version")]
		public int FormatVersion;
		[NonSerialized]
		public string Hash;
		[NonSerialized]
		public string Path;
	}
	[Serializable, JsonObject(MemberSerialization.OptIn)]
	public class BeatmapSet : BeatmapSetInfo
	{
		[JsonProperty("_artist")]
		public string Artist;
		[JsonProperty("_title")]
		public string Title;
		public string Name
		{
			get
			{
				return $"{Artist} - {Title}";
			}
		}
		[JsonProperty("_difficulties")]
		public List<string> _difficulties;
		public List<Beatmap> Difficulties;
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
		public static BeatmapSet Load(string json)
		{
			var version = JsonConvert.DeserializeObject<BeatmapSetInfo>(json);
			BeatmapSet map;
			switch (version.FormatVersion)
			{
				default:
					map = JsonConvert.DeserializeObject<BeatmapSet>(json);
					break;
			}
			return map;
		}
		public static BeatmapSet LoadFromPath(string path, string hash)
		{
			path = path.Replace("user://", OS.GetUserDataDir());
			var file = new File();
			if (file.Open(path.PlusFile("cache.bin"), File.ModeFlags.Read) == Error.Ok)
			{
				// GD.Print("Loading map from cache: " + path);
				var deserializer = new BinaryFormatter();
				var buffer = file.GetBuffer((long)file.GetLen());
				var stream = new MemoryStream(buffer);
				var cachedMap = (BeatmapSet)deserializer.Deserialize(stream);
				cachedMap.Path = path;
				cachedMap.Hash = hash;
				foreach (Beatmap difficulty in cachedMap.Difficulties)
				{
					difficulty.Mapset = cachedMap;
				}
				return cachedMap;
			}
			// GD.Print("Loading map without cache: " + path);
			file.Open(path.PlusFile("meta.json"), File.ModeFlags.Read);
			var map = BeatmapSet.Load(file.GetAsText());
			map.Path = path;
			map.Difficulties = new List<Beatmap>();
			foreach (string difficulty in map._difficulties)
			{
				var diffFile = new File();
				diffFile.Open(path.PlusFile(difficulty), File.ModeFlags.Read);
				var diff = JsonConvert.DeserializeObject<Beatmap>(diffFile.GetAsText());
				diff.Path = difficulty;
				diff.Mapset = map;
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
}