using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Utility;

namespace Game.ZigZag.Configs
{
	

	[Serializable]
	public class DifficultyConfigs
	{
		public int Difficulty;

		public LevelConfig LevelConfig;
	}

	[CreateAssetMenu(fileName = "FieldConfig", menuName = "ZigZag/FieldConfig", order = -1)]
	public class GameConfig: ScriptableObject, IGameConfig
	{
		public int Difficulty => _difficulty;

		public List<DifficultyConfigs> DifficultyConfigs => _difficultyConfigs;

		[SerializeField]
		private int _difficulty;

		[SerializeField]
		private List<DifficultyConfigs> _difficultyConfigs;

		public ILevelConfig GetCurrentLevelConfig()
		{
			return DifficultyConfigs.FirstOrDefault(c => c.Difficulty == Difficulty)?.LevelConfig;
		}
	}
}
