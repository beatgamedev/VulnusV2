using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Gameplay.Mods
{
	public class ModList : List<Mod>
	{
		public new void Add(Mod mod)
		{
			foreach (Mod other in this.Where(m => m.GetType() == mod.GetType()))
				this.Remove(other);
			foreach (Mod other in this.Where(m => !mod.CompatibleWith(m)))
				this.Remove(other);
			base.Add(mod);
		}
		public new string ToString()
		{
			return Count == 0 ? "None" : string.Join(", ", this);
		}
	}
}