using Godot;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

public static class Settings
{
	public static int CameraMode = 0;
	public static float MouseSensitivity = 1f;
	public static int ApproachMode = 0;
	public static float ApproachDistance = 50f;
	public static float ApproachTime = 1f;
	public static float ApproachRate = 50f;
	public static int[] Volume = new int[3];
	public static void UpdateSettings(bool loading)
	{
		if (loading)
			LoadSettings();
		else
			SaveSettings();
		var masterBus = AudioServer.GetBusIndex("Master");
		AudioServer.SetBusVolumeDb(masterBus, GD.Linear2Db(Volume[0] / 100f));
		var musicBus = AudioServer.GetBusIndex("Music");
		AudioServer.SetBusVolumeDb(musicBus, GD.Linear2Db(Volume[1] / 100f));
		var sfxBus = AudioServer.GetBusIndex("SFX");
		AudioServer.SetBusVolumeDb(sfxBus, GD.Linear2Db(Volume[2] / 100f));
	}
	public static void LoadSettings()
	{
		var path = OS.GetUserDataDir().PlusFile("settings.bin");
		var file = new Godot.File();
		var settings = new SerializedSettings();
		if (file.FileExists(path))
		{
			file.Open(path, Godot.File.ModeFlags.Read);
			var deserializer = new BinaryFormatter();
			var buffer = file.GetBuffer((long)file.GetLen());
			var stream = new MemoryStream(buffer);
			settings = (SerializedSettings)deserializer.Deserialize(stream);
			file.Close();
		}
		foreach (FieldInfo field in typeof(SerializedSettings).GetFields())
		{
			typeof(Settings).GetField(field.Name).SetValue(null, field.GetValue(settings));
		}
	}
	public static void SaveSettings()
	{
		var writer = new FileStream(OS.GetUserDataDir().PlusFile("settings.bin"), FileMode.Create);
		var serializer = new BinaryFormatter();
		var settings = new SerializedSettings();
		foreach (FieldInfo field in typeof(SerializedSettings).GetFields())
		{
			field.SetValue(settings, typeof(Settings).GetField(field.Name).GetValue(null));
		}
		serializer.Serialize(writer, settings);
		writer.Flush();
		writer.Dispose();
	}
	[Serializable]
	private class SerializedSettings
	{
		public int CameraMode = 0;
		public float MouseSensitivity = 1f;
		public int ApproachMode = 0;
		public float ApproachDistance = 50f;
		public float ApproachTime = 1f;
		public float ApproachRate = 50f;
		public int[] Volume = new int[3] { 25, 25, 25 };
	}
}
