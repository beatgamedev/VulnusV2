using Godot;
using System;

public class SyncManager : Node
{
	public Game Game;

	public NoteManager NoteManager;

	public AudioStreamPlayer AudioPlayer;

	public double NoteTime;
	public double SongTime;
	public double SkippedTime;
	public bool SongPlaying;
	public double SongPlayingAt;
	public double SongPlayingOffset;

	public override void _Ready()
	{
		Game = GetParent<Game>();

		NoteManager = Game.GetNode<NoteManager>("NoteManager");

		AudioPlayer = GetNode<AudioStreamPlayer>("AudioPlayer");
		AudioPlayer.Connect("finished", this, nameof(AudioEnded));

		NoteTime = 0f;
		SongTime = -1f;
		SkippedTime = 0;
	}
	public override void _Process(float delta)
	{
		if (Game.Ended)
			return;
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
			SongTime = Math.Max(0.0, songTime - SongPlayingOffset) + SkippedTime;
			var difference = SongTime - (AudioPlayer.GetPlaybackPosition() + AudioServer.GetTimeSinceLastMix());
			if (difference > 0.002)
			{
				GD.Print($"Resynced! {Math.Round(difference * 1000)}ms");
				SongPlayingOffset += difference;
			}
		}
		if (SongPlaying && !AudioPlayer.Playing)
			AudioPlayer.Play();
		NoteTime = SongTime;
	}
	public override void _Input(InputEvent @event)
	{
		if (!(@event is InputEventAction))
			return;
		var input = @event as InputEventAction;
		if (input.Action == "skip")
		{
			var skippableTime = SkippableTime();
			if (CanSkip())
			{
				SkippedTime += skippableTime;
				AudioPlayer.Seek((float)(SongTime + skippableTime));
				SongTime += skippableTime;
			}
		}
	}
	public bool CanSkip()
	{
		if (NoteManager.LastNote == null || NoteManager.NextNote == null)
			return SkippableTime() >= 2f;
		return SkippableTime() >= 2f && (NoteManager.NextNote.T - NoteManager.LastNote.T) > 5f;
	}
	public double SkippableTime()
	{
		if (NoteManager.NextNote == null)
			return (double)AudioPlayer.Stream.GetLength() - NoteTime - 1f;
		return NoteManager.NextNote.T - NoteTime - 1f;
	}
	[Signal]
	public delegate void Ended();
	public void AudioEnded()
	{
		EmitSignal(nameof(Ended));
	}
	public void SetStream(AudioStream stream)
	{
		AudioPlayer.Stop();
		AudioPlayer.Stream = stream;
	}
	private void PlayAudio()
	{
		SongPlayingAt = GetTimeSeconds();
		SongPlayingOffset = GetAudioDelay();
		AudioPlayer.Play((float)SongTime);
	}
	private double GetTimeSeconds()
	{
		return OS.GetTicksUsec() / 1000000.0d;
	}
	private double GetAudioDelay()
	{
		return AudioServer.GetTimeToNextMix() + AudioServer.GetOutputLatency();
	}
}
