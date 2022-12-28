using Godot;
using System;
using System.Linq;

namespace Gameplay.Mods
{
	public abstract class Mod : IEquatable<Mod>
	{
		public abstract string Name { get; }
		public abstract string Acronym { get; }
		public abstract string Description { get; }
		public abstract double ScoreMultiplier { get; }
		public abstract double PerformanceMultiplier { get; }
		public abstract Type[] IncompatibleMods { get; }
		public bool CompatibleWith(Mod other)
		{
			return IncompatibleMods.Any(t => t.IsInstanceOfType(other)) || other.IncompatibleMods.Any(t => t.IsInstanceOfType(this));
		}
		public virtual ModType Type { get; } = ModType.Misc;
		public bool Equals(Mod other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return GetType() == other.GetType();
		}
		public new string ToString()
		{
			return Acronym;
		}
	}

	public enum ModType
	{
		Buff,
		Nerf,
		Misc
	}
}