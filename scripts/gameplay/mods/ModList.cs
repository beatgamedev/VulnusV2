using Godot;
using System;
using System.Collections.Generic;

public class ModList : List<Mod>
{
	public new void Add(Mod mod)
	{
		if (this.Contains(mod)) return;
		List<Mod> removals = new List<Mod>();
		foreach (Mod other in this)
		{
			if (!mod.CompatibleWith(other))
				removals.Add(other);
		}
		foreach (Mod other in removals)
			this.Remove(other);
		base.Add(mod);
	}
	public new string ToString()
	{
		return Count == 0 ? "NM" : string.Join(", ", this);
	}
}