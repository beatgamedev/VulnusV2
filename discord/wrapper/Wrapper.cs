using Godot;
using System;

namespace Discord
{
	public class Wrapper
	{
		private static bool Disabled = true;
		private static long ClientId = 954573999088730154;
		public Discord discord { get; private set; } = new Discord(ClientId, (ulong)CreateFlags.NoRequireDiscord);
		public bool Started { get; private set; } = false;
		public bool Disposed { get; private set; } = false;
		public void StartDebug()
		{
			if (Started || Disabled) return;
			Started = true;
			discord?.SetLogHook(LogLevel.Debug, (level, message) =>
			{
				GD.Print("[DISCORD] " + level + " || " + message);
			});
			discord?.SetLogHook(LogLevel.Error, (level, message) =>
			{
				var msg = "[DISCORD] " + level + " || " + message;
				GD.PushError(msg);
				GD.PrintErr(msg);
			});
			discord?.SetLogHook(LogLevel.Warn, (level, message) =>
			{
				var msg = "[DISCORD] " + level + " || " + message;
				GD.PushWarning(msg);
				GD.Print(msg);
			});
			discord?.SetLogHook(LogLevel.Info, (level, message) =>
			{
				GD.Print("[DISCORD] " + level + " || " + message);
			});
		}
		public void Dispose()
		{
			if (Disposed) return;
			Disposed = true;
			discord?.Dispose();
			discord = null;
		}
		public void SetActivity(string state = "Idle", string details = null, long? startTimestamp = null, long? endTimestamp = null)
		{
			if (discord == null) return;
			var activityManager = discord.GetActivityManager();
			var activity = new Activity();
			activity.ApplicationId = ClientId;
			activity.Assets.LargeImage = "vulnus";
			activity.Assets.LargeText = "Vulnus";
			activity.Assets.SmallImage = "beatgamedev";
			if (startTimestamp != null) activity.Timestamps.Start = (long)startTimestamp;
			if (endTimestamp != null) activity.Timestamps.End = (long)endTimestamp;
			activity.State = state;
			activity.Details = details;
			activityManager.UpdateActivity(activity, (result) => { });
		}
		public void RunCallbacks()
		{
			if (Started && !Disabled)
				discord?.RunCallbacks();
		}
	}
}