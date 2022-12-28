using Godot;
using System;
using System.Collections.Generic;

namespace Gameplay
{
	public class Score
	{
		public int Points = 0;
		public int Misses = 0;
		public int Total = 0;
		public int Combo = 0;
		public int HighestCombo = 0;
		public int Multiplier = 1;
		public int Miniplier = 0;
		public double Health = 10;
		public bool Failed = false;
		public static Dictionary<double, string> Ranks = new Dictionary<double, string>
	{
		{99.0,"SS"},
		{97.5,"S"},
		{95.0,"A"},
		{90.0,"B"},
		{85.0,"C"},
		{75.0,"D"},
		{0.0, "E"}
	};
		public static string GetRankForAccuracy(double accuracy)
		{
			foreach (KeyValuePair<double, string> pair in Ranks)
			{
				if (accuracy * 100 >= pair.Key)
				{
					return pair.Value;
				}
			}
			return Ranks[0.0];
		}
	}
}