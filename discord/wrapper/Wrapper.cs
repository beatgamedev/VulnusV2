using Godot;
using System;

namespace Discord
{
	public class DiscordW
	{
		public static long ClientId { get; private set; } = 954573999088730154;
		public bool Disposed { get; private set; } = false;
		private Discord Discord = new Discord(ClientId, (ulong)CreateFlags.NoRequireDiscord);
		public void Dispose()
		{
			if (Disposed) return;
			Disposed = true;
			this.Discord?.Dispose();
			this.Discord = null;
		}
		public void SetActivity(ActivityW activity)
		{
			if (this.Discord == null) return;
			var activityManager = this.Discord.GetActivityManager();
			activityManager.UpdateActivity(activity.Activity, (result) => { });
		}
		public void RunCallbacks()
		{
			if (this.Discord == null) return;
			try
			{
				this.Discord?.RunCallbacks();
			}
			catch
			{
				Dispose();
			}
		}
	}
	public class ActivityW
	{
		public Activity Activity { get; private set; }
		public DateTime? StartTimestamp
		{
			get
			{
				if (this.Activity.Timestamps.Start < 1) return null;
				return DateTimeOffset.FromUnixTimeSeconds(this.Activity.Timestamps.Start).DateTime;
			}
			set
			{
				var activity = this.Activity;
				if (value != null)
					activity.Timestamps.Start = ((DateTimeOffset)value).ToUnixTimeSeconds();
			}
		}
		public DateTime? EndTimestamp
		{
			get
			{
				if (this.Activity.Timestamps.End < 1) return null;
				return DateTimeOffset.FromUnixTimeSeconds(this.Activity.Timestamps.End).DateTime;
			}
			set
			{
				var activity = this.Activity;
				if (value != null)
					activity.Timestamps.End = ((DateTimeOffset)value).ToUnixTimeSeconds();
			}
		}
		public string State
		{
			get => this.Activity.State;
			set
			{
				var activity = this.Activity;
				activity.State = value;
			}
		}
		public string Details
		{
			get => this.Activity.Details;
			set
			{
				var activity = this.Activity;
				activity.Details = value;
			}
		}
		public ActivityW(string state = "Idle", string details = null, DateTime? startTimestamp = null, DateTime? endTimestamp = null)
		{
			var activity = new Activity();
			activity.ApplicationId = DiscordW.ClientId;
			activity.Assets.LargeImage = "vulnus";
			activity.Assets.LargeText = "Vulnus";
			activity.Assets.SmallImage = "beatgamedev";
			if (startTimestamp != null) activity.Timestamps.Start = ((DateTimeOffset)startTimestamp).ToUnixTimeSeconds();
			if (endTimestamp != null) activity.Timestamps.End = ((DateTimeOffset)endTimestamp).ToUnixTimeSeconds();
			activity.State = state;
			activity.Details = details;
			this.Activity = activity;
			this.StartTimestamp = startTimestamp;
			this.EndTimestamp = endTimestamp;
			this.State = state;
			this.Details = details;
		}
	}
}