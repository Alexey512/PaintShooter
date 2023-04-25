using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.ZigZag.Configs
{
	public enum RewardRule
	{
		Undefined,
		Random,
		Sequence
	}
	
	[Serializable]
	public struct TileConfig
	{
		public int Width;
		public int Height;
	}


	[Serializable]
	public struct RewardsConfig
	{
		public TileConfig Tile;


	}

	[Serializable]
	public struct DifficultyConfig
	{

	}

	[CreateAssetMenu(fileName = "FieldConfig", menuName = "Zag/FieldConfig", order = -1)]
	public class GameConfig: ScriptableObject
	{
		public float BallSpeed;

		public RewardRule RewardRule;

		public DifficultyConfig DifficultyConfig;
	}
}
