using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.ZigZag.Configs
{
	public enum RewardRule
	{
		Undefined,
		Random,
		Sequence
	}
	
	[Serializable]
	public class TileSegmentChance
	{
		public float Chance;

		public int Length;

		public override string ToString()
		{
			return $"chance:{Chance}; length:{Length}";
		}
	}

	public interface ILevelConfig
	{
		float BallSpeed { get; }

		RewardRule RewardRule { get; }

		int TileSize { get; }

		List<TileSegmentChance> SegmentsChances { get; }
	}
}
