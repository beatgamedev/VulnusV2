using Godot;
using System;
using System.Threading;
using System.Threading.Tasks;
using Squirrel;

public static class SquirrelW
{
	private static UpdateManager updateManager;
	public static EventsW Events = new EventsW();
	public static bool UpToDate = true;
	public static void Run()
	{
		SquirrelAwareApp.HandleEvents(
			onInitialInstall: OnInstall,
			onAppUninstall: OnUninstall,
			onEveryRun: OnRun
		);
		updateManager = new UpdateManager("https://vulnus.net/dl/game");
	}
	public static async Task Update()
	{
		if (UpToDate) return;
		await updateManager.UpdateApp(new Action<int>((int progress) => Events.EmitSignal(nameof(EventsW.OnProgress), progress)));
		UpdateManager.RestartApp();
	}
	private static async Task CheckForUpdates()
	{
		if (!UpToDate) return;
		var newVer = await updateManager.CheckForUpdate();
		if (newVer != null)
		{
			UpToDate = false;
			Events.EmitSignal(nameof(EventsW.OnOutdated), newVer.FutureReleaseEntry.Version.ToString());
		}
	}
	private static void OnInstall(SemanticVersion version, IAppTools tools)
	{
		tools.CreateShortcutForThisExe(ShortcutLocation.StartMenu | ShortcutLocation.Desktop);

	}
	private static void OnUninstall(SemanticVersion version, IAppTools tools)
	{
		tools.RemoveShortcutForThisExe(ShortcutLocation.StartMenu | ShortcutLocation.Desktop);
	}
	private static void OnRun(SemanticVersion version, IAppTools tools, bool firstRun)
	{
		tools.SetProcessAppUserModelId();
	}
	public class EventsW : Reference
	{
		[Signal]
		public delegate void OnProgress(int progress);
		[Signal]
		public delegate void OnOutdated(string newVersion);
	}
}
