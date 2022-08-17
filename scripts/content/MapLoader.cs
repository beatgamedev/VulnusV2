using Godot;
using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using File = Godot.File;
using Directory = Godot.Directory;
public static class MapLoader
{
	public static List<Map> LoadedMaps = new List<Map>();
	public static bool LoadMapsFromDirectory(string directory)
	{
		GD.Print("Loading maps from " + directory);
		// Get a list of cached maps
		var cachePath = directory.PlusFile(".cache");
		var cacheDir = new Directory();
		if (cacheDir.Open(cachePath) != Error.Ok)
		{
			cacheDir.MakeDirRecursive(cachePath);
			cacheDir.Open(cachePath);
		}
		List<string> caches = new List<string>();
		cacheDir.ListDirBegin(true, true);
		var cacheFileName = cacheDir.GetNext();
		while (cacheFileName != "")
		{
			if (cacheDir.CurrentIsDir())
			{
				caches.Add(cacheFileName);
				// GD.Print("Found cache: " + cacheFileName);
			}
			cacheFileName = cacheDir.GetNext();
		}
		// Get a list of existing maps
		var hashes = new List<string>();
		var mapsDir = new Directory();
		mapsDir.Open(directory);
		mapsDir.ListDirBegin(true, true);
		var mapFileName = mapsDir.GetNext();
		var mapFile = new File();
		while (mapFileName != "")
		{
			if (mapFileName.Extension() == "vul")
			{
				var hash = mapFile.GetMd5(directory.PlusFile(mapFileName));
				hashes.Add(hash);
				if (LoadedMaps.Find(map => map.Hash == hash) != null)
				{
					// GD.Print("Map already loaded: " + hash);
				}
				else
				{
					if (!caches.Contains(hash)) // If the map isn't already cached, do extraction stuff
					{
						mapFile.Open(directory.PlusFile(mapFileName), File.ModeFlags.Read);
						var stream = new MemoryStream(mapFile.GetBuffer((long)mapFile.GetLen()));
						ZipArchive zip = new ZipArchive(stream, ZipArchiveMode.Read);
						zip.ExtractToDirectory(cachePath.PlusFile(hash));
					}
					LoadedMaps.Add(Map.LoadFromPath(cachePath.PlusFile(hash), hash)); // Load map from cache
				}
			}
			mapFileName = mapsDir.GetNext();
		}
		// Delete old caches
		foreach (string hash in caches)
		{
			if (!hashes.Contains(hash))
			{
				System.IO.Directory.Delete(cachePath.PlusFile(hash), true);
			}
		}
		if (mapFile.IsOpen()) mapFile.Close();
		return true;
	}
}
