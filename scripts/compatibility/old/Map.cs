using Godot;
using System;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;

using File = System.IO.File;
using Directory = System.IO.Directory;

namespace Compatibility.Old
{
	public static class Map
	{
		public static void ImportFolder(string path)
		{
			if (!Directory.Exists(path))
				return;
			if (!File.Exists(path.PlusFile("meta.json")))
				return;
			var importPath = Global.MapPath.PlusFile($"{path.GetHashCode()}.tmp");
			if (!Directory.Exists(importPath.GetBaseDir()))
				return;
			ZipFile.CreateFromDirectory(path, importPath);
			var md5 = new Godot.File().GetMd5(importPath);
			if (md5 == "")
				return;
			var newPath = Global.MapPath.PlusFile($"old_{md5}.vul");
			File.Move(importPath, newPath);
		}
	}
}