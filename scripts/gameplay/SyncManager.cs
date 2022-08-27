using Godot;
using System;

public class SyncManager : Node
{
	public Game Game;
	public GameCamera Camera;

	public AudioStreamPlayer AudioPlayer;

	public double NoteTime;
	public double SongTime;
	public bool SongPlaying;
	public ulong SongPlayingAt;
	public double SongPlayingOffset;

	public override void _Ready()
	{
		AudioPlayer = GetNode<AudioStreamPlayer>("AudioPlayer");
		NoteTime = 0f;
		SongTime = -1f;
	}
	private ulong GetTimeSeconds()
	{
		return OS.GetTicksUsec();
	}
	private double GetAudioDelay()
	{
		return AudioServer.GetTimeToNextMix() + AudioServer.GetOutputLatency();
	}
	public void PlayAudio()
	{
		SongPlayingAt = GetTimeSeconds();
		SongPlayingOffset = GetAudioDelay();
		AudioPlayer.Play((float)SongTime);
	}
	public void SetStream(AudioStream stream)
	{
		AudioPlayer.Stop();
		AudioPlayer.Stream = stream;
	}
	public override void _Process(float delta)
	{
		if (!SongPlaying)
		{
			SongTime += delta;
			if (SongTime >= 0)
			{
				SongPlaying = true;
				PlayAudio();
			}
		}
		else
		{
			var songTime = GetTimeSeconds() - SongPlayingAt;
			SongTime = Math.Max(0.0, songTime - SongPlayingOffset);
			var difference = SongTime - (AudioPlayer.GetPlaybackPosition() + AudioServer.GetTimeSinceLastMix());
			if (Math.Abs(difference) > 0.002)
			{
				GD.Print($"Resynced! {Math.Round(difference * 1000)}ms");
				SongPlayingOffset -= difference;
			}
		}
		if (SongPlaying && !AudioPlayer.Playing)
			AudioPlayer.Play();
		NoteTime = SongTime;
	}
}
