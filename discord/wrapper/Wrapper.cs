using Godot;
using System;

namespace Discord
{
	public class DiscordW
	{
		private static bool Disabled = false;
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
			if (!Disabled)
				this.Discord?.RunCallbacks();
		}
	}
	public class ActivityW
	{
		public Activity Activity { get; private set; }
		public ActivityW(string state = "Idle", string details = null, long? startTimestamp = null, long? endTimestamp = null)
		{
			var activity = new Activity();
			activity.ApplicationId = DiscordW.ClientId;
			activity.Assets.LargeImage = "vulnus";
			activity.Assets.LargeText = "Vulnus";
			activity.Assets.SmallImage = "beatgamedev";
			if (startTimestamp != null) activity.Timestamps.Start = (long)startTimestamp;
			if (endTimestamp != null) activity.Timestamps.End = (long)endTimestamp;
			activity.State = state;
			activity.Details = details;
			this.Activity = activity;
		}
	}
}