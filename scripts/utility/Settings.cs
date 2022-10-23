using Godot;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public static class Settings
{
	public static bool AnyPause = false;
	public static int CameraMode = 0;
	public static float MouseSensitivity = 1f;
	public static int ApproachMode = 0;
	public static float ApproachDistance = 50f;
	public static float ApproachTime = 1f;
	public static float ApproachRate = 50f;
	public static int[] Volume = new int[3];
	public static float RenderScale = 1f;
	public static float UIScale = 1f;
	public static bool CursorDrift = false;
	public static int Bloom = 0;
	public static int FPSLimit = 0;
	public static bool VSync = false;
	public static bool Debanding = true;
	public static void UpdateSettings(bool loading = false)
	{
		if (loading)
			LoadSettings();
		else
			SaveSettings();
		Global.Instance.ViewportChanged();
		OS.VsyncEnabled = VSync;
		Engine.TargetFps = FPSLimit;
		switch (ApproachMode)
		{
			case 0:
				ApproachRate = ApproachDistance / ApproachTime;
				break;
			case 2:
				ApproachDistance = ApproachRate * ApproachTime;
				break;
			case 1:
				ApproachTime = ApproachDistance / ApproachRate;
				break;
			default:
				break;
		}
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
		settings.SetDefaults();
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
			try
			{
				var value = field.GetValue(settings);
				typeof(Settings).GetField(field.Name).SetValue(null, value);
			}
			catch (Exception e)
			{
				GD.Print($"Failed loading {field.Name}: {e.Message}");
			}
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
		public bool AnyPause;
		public int CameraMode;
		public float MouseSensitivity;
		public int ApproachMode;
		public float ApproachDistance;
		public float ApproachTime;
		public float ApproachRate;
		public int[] Volume;
		[OptionalField(VersionAdded = 2)]
		public float RenderScale;
		[OptionalField(VersionAdded = 2)]
		public float UIScale;
		[OptionalField(VersionAdded = 2)]
		public bool CursorDrift;
		[OptionalField(VersionAdded = 2)]
		public int Bloom;
		[OptionalField(VersionAdded = 2)]
		public int FPSLimit;
		[OptionalField(VersionAdded = 2)]
		public bool VSync;
		[OptionalField(VersionAdded = 2)]
		public bool Debanding;
		[OnDeserializing()]
		internal void OnDeserializing(StreamingContext ctx)
		{
			SetDefaults();
		}
		public void SetDefaults()
		{
			AnyPause = false;
			CameraMode = 0;
			MouseSensitivity = 1f;
			ApproachMode = 0;
			ApproachDistance = 50f;
			ApproachTime = 1f;
			ApproachRate = 50f;
			Volume = new int[3] { 25, 25, 25 };
			RenderScale = 1f;
			UIScale = 1f;
			CursorDrift = false;
			Bloom = 0;
			FPSLimit = 0;
			VSync = false;
			Debanding = true;
		}
	}
}
