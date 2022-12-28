using Godot;
using System;

namespace Gameplay.Mods
{
	public interface IApplicableMod { }
	public interface IApplicableToGame : IApplicableMod
	{
		void ApplyToGame(Game game);
	}
	public interface IApplicableToNote : IApplicableMod
	{
		void ApplyToNote(Note note);
	}
	public interface IApplicableToNoteManager : IApplicableMod
	{
		void ApplyToNoteManager(NoteManager noteManager);
	}
	public interface IApplicableToNoteRenderer : IApplicableMod
	{
		void ApplyToNoteManager(NoteRenderer noteRenderer);
	}
	public interface IApplicableToSyncManager : IApplicableMod
	{
		void ApplyToSyncManager(SyncManager syncManager);
	}
	public interface IApplicableToHUDManager : IApplicableMod
	{
		void ApplyToHUDManager(HUDManager hudManager);
	}
}