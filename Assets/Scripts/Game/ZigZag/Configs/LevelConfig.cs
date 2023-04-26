using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.ZigZag.Configs
{
	
	[CreateAssetMenu(fileName = "LevelConfig", menuName = "ZigZag/Level Config", order = -1)]
	public class LevelConfig: ScriptableObject, ILevelConfig
	{
		public float BallSpeed => _ballSpeed;

		public RewardRule RewardRule => _rewardRule;
		
		public int TileSize => _tileSize;

		public List<TileSegmentChance> SegmentsChances => _segmentsChances;

		[SerializeField] 
		private float _ballSpeed;

		[SerializeField] 
		private RewardRule _rewardRule;

		[SerializeField]
		private int _tileSize;

		[SerializeField] 
		private List<TileSegmentChance> _segmentsChances = new List<TileSegmentChance>();
	}
}
