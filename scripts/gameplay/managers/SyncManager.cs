using Godot;
using System;

namespace Gameplay
{
	public class SyncManager : Node
	{
		public Game Game;

		public NoteManager NoteManager;

		public AudioStreamPlayer AudioPlayer;

		public double NoteTime;
		public double SongTime;
		public bool SongPlaying;
		public double SongPlayingAt;
		public double SongPlayingOffset;

		public float Speed = 1f;

		public Action Ended = () => { };

		public override void _Ready()
		{
			Game = GetParent<Game>();

			NoteManager = Game.GetNode<NoteManager>("NoteManager");

			AudioPlayer = GetNode<AudioStreamPlayer>("AudioPlayer");
			AudioPlayer.Connect("finished", this, nameof(AudioEnded));

			NoteTime = 0f;
			SongTime = -1f;
		}
		public override void _Process(float delta)
		{
			if (Game.Ended)
				return;
			if (!SongPlaying)
			{
				SongTime += delta * Speed;
				if (SongTime >= 0)
				{
					SongPlaying = true;
					PlayAudio();
				}
			}
			else
			{
				var songTime = (GetTimeSeconds() - SongPlayingAt) * Speed;
				SongTime = Math.Max(0.0, songTime - SongPlayingOffset);
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
		public void AttemptSkip()
		{
			if (CanSkip())
			{
				var skippableTime = SkippableTime();
				GD.Print($"Attempting to skip {SkippableTime()}s");
				if (SongPlaying)
				{
					SongPlayingAt -= skippableTime / Speed;
					AudioPlayer.Seek((float)(SongTime + skippableTime));
				}
				SongTime += skippableTime;
			}
		}
		public bool CanSkip()
		{
			if (NoteManager.LastNote == null || NoteManager.NextNote == null)
				return SkippableTime() >= 5f * Speed;
			return SkippableTime() >= 2f * Speed && (NoteManager.NextNote.T - NoteManager.LastNote.T) > 5f * Speed;
		}
		public double SkippableTime()
		{
			var skipToEnd = (double)AudioPlayer.Stream.GetLength() - NoteTime - Speed;
			if (NoteManager.LastNote == null && NoteManager.NextNote == null)
				return skipToEnd;
			if (NoteManager.LastNote != null && NoteManager.LastNote.Index == (NoteManager.Notes.Count - 1))
				return skipToEnd;
			return NoteManager.NextNote.T - NoteTime - Speed;
		}
		public void AudioEnded()
		{
			Ended();
		}
		public void SetStream(AudioStream stream)
		{
			AudioPlayer.Stop();
			AudioPlayer.Stream = stream;
		}
		private void PlayAudio()
		{
			SongPlayingAt = GetTimeSeconds() - SongTime;
			SongPlayingOffset = GetAudioDelay();
			AudioPlayer.PitchScale = Speed;
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
}