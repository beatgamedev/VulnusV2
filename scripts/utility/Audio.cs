using Godot;
using System;

public static class AudioHandler
{
	public static AudioStream LoadAudio(string path)
	{
		var file = new File();
		file.Open(path, File.ModeFlags.Read);
		var buffer = file.GetBuffer((long)file.GetLen());
		file.Close();
		AudioStream stream;
		switch (GetFileFormat(buffer))
		{
			case "mp3":
				stream = new AudioStreamMP3();
				((AudioStreamMP3)stream).Data = buffer;
				break;
			case "ogg":
				stream = new AudioStreamOGGVorbis();
				((AudioStreamOGGVorbis)stream).Data = buffer;
				break;
			default:
				stream = new AudioStreamSample();
				((AudioStreamSample)stream).Data = buffer;
				break;
		}
		return stream;
	}
	private static string GetFileFormat(byte[] bytes)
	{
		if (bytes.Length < 10) return "unknown";
		if (bytes[0] == 0x52 && bytes[1] == 0x49 && bytes[2] == 0x46 && bytes[3] == 0x46) return "wav";
		if ((bytes[0] == 0xFF && (bytes[1] == 0xFB || (bytes[1] == 0xFA && bytes[2] == 0x90))) || (bytes[0] == 0x49 && bytes[1] == 0x44 && bytes[2] == 0x33)) return "mp3";
		if (bytes[0] == 0x4F && bytes[1] == 0x67 && bytes[2] == 0x67 && bytes[3] == 0x53) return "ogg";
		return "unknown";
	}
}
