using Godot;
using System;

namespace Gameplay.Mods
{
	public class ModNoFail : Mod, IApplicableToGame
	{
		public override string Name => "No Fail";
		public override string Acronym => "NF";
		public override string Description => "No more failing";
		public override double ScoreMultiplier => 0.5;
		public override double PerformanceMultiplier => 0;
		public override Type[] IncompatibleMods => throw new NotImplementedException();
		public override ModType Type => ModType.Misc;
		public void ApplyToGame(Game game)
		{
			game.CanFail = false;
		}
	}
}