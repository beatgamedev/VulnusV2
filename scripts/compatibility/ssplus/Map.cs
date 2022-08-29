using Godot;
using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using File = Godot.File;

namespace Compatibility.SSP
{
	public static class Map
	{
		public static void Import(string path)
		{
			var file = new File();
			var err = file.Open(path, File.ModeFlags.Read);
			if (err != Error.Ok)
				return;
			var header = file.GetBuffer(4);
			if (header.GetStringFromASCII() != "SS+m")
				return;
			if (file.Get16() > 1)
				return;
			if (file.Get16() != 0)
				return;
			file.GetLine();
			var md5 = file.GetMd5(path);
			var importPath = Global.MapPath.PlusFile($"sspm_{md5}.vul");
			using (var stream = new FileStream(importPath, FileMode.CreateNew))
			{
				using (var zip = new ZipArchive(stream, ZipArchiveMode.Create))
				{
					var metaEntry = zip.CreateEntry("meta.json");
					var metaJson = new JObject();
					metaJson.Add("_version", 1);
					metaJson.Add("_title", file.GetLine());
					metaJson.Add("_mappers", new JArray(file.GetLine()));
					metaJson.Add("_difficulties", new JArray("extracted.json"));
					metaJson.Add("_music", "music.bin");
					var metaBytes = System.Text.Encoding.ASCII.GetBytes(metaJson.ToString());
					var metaStream = metaEntry.Open();
					metaStream.Write(metaBytes, 0, metaBytes.Length);
					metaStream.Flush();
					metaStream.Close();
					file.Get32();
					var noteCount = file.Get32();
					file.Get8();
					var imageType = file.Get8();
					if (imageType == 1 || imageType == 2)
					{
						var imageEntry = zip.CreateEntry("cover.png");
						var imageStream = imageEntry.Open();
						if (imageType == 1)
						{
							var image = new Image();
							var height = file.Get16();
							var width = file.Get16();
							file.Get8();
							var format = file.Get8();
							var contentLength = file.Get64();
							var content = file.GetBuffer((long)contentLength);
							image.CreateFromData(width, height, false, (Image.Format)format, content);
							var imageBuffer = image.SavePngToBuffer();
							imageStream.Write(imageBuffer, 0, imageBuffer.Length);
							image.Free();
						}
						else
						{
							var contentLength = file.Get64();
							var content = file.GetBuffer((long)contentLength);
							imageStream.Write(content, 0, (int)contentLength);
						}
						imageStream.Flush();
						imageStream.Close();
					}
					file.Get8();
					var musicEntry = zip.CreateEntry("music.bin");
					var musicLength = file.Get64();
					var musicBuffer = file.GetBuffer((long)musicLength);
					var musicStream = musicEntry.Open();
					musicStream.Write(musicBuffer, 0, (int)musicLength);
					musicStream.Flush();
					musicStream.Close();
					var notesEntry = zip.CreateEntry("extracted.json");
					var notesJson = new JObject();
					notesJson.Add("_version", 1);
					notesJson.Add("_name", "Sound Space Plus");
					var notes = new JArray();
					for (int i = 0; i < noteCount; i++)
					{
						var note = new JObject();
						note.Add("_time", (float)(file.Get32()) / 1000f);
						if (file.Get8() == 0)
						{
							note.Add("_x", -(file.Get8() - 1));
							note.Add("_y", -(file.Get8() - 1));
						}
						else
						{
							note.Add("_x", -(file.GetFloat() - 1f));
							note.Add("_y", -(file.GetFloat() - 1f));
						}
						notes.Add(note);
					}
					notesJson.Add("_notes", notes);
					var notesBytes = System.Text.Encoding.ASCII.GetBytes(notesJson.ToString());
					var notesStream = notesEntry.Open();
					notesStream.Write(notesBytes, 0, notesBytes.Length);
					notesStream.Flush();
					notesStream.Close();
				}
			}
			file.Close();
		}
	}
}
