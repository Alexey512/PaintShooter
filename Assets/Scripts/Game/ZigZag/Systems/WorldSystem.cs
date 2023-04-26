using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECS;
using Game.Core.Factories;
using Game.ZigZag.Components;
using Game.ZigZag.Configs;
using Game.ZigZag.Factories;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;
using Random = UnityEngine.Random;

namespace Game.ZigZag.Systems
{
	[Serializable]
	public class WorldSystem: BaseSystem, IAfterEntityInitialize
	{
		[Inject]
		private IActorsFactory<ActorType> _actorsFactory;

		[Inject]
		private IGameConfig _gameConfig;

		private ILevelConfig _levelConfig;

		private PlayerComponent _player;

		private List<TileComponent> _tiles = new List<TileComponent>();

		private List<KeyValuePair<float, int>> _segmentsWeight = new List<KeyValuePair<float, int>>();

		private Vector2 _lastPosition = Vector2.zero;
		
		private Vector2 _lastDirection = Vector2.zero;

		public void AfterEntityInitialize()
		{
			_levelConfig = _gameConfig.GetCurrentLevelConfig();
			if (_levelConfig == null)
				return;
			
			//_player = EntityManager.Instance.GetComponents<PlayerComponent>().FirstOrDefault();
			//if (_player == null)
			//	return;

			InitializeSegmentsWeight();

			CreatePath(100);
		}

		private void InitializeSegmentsWeight()
		{
			if (_levelConfig == null)
				return;
			
			_segmentsWeight.Clear();

			float sumChances = _levelConfig.SegmentsChances.Sum(s => s.Chance);
			if (sumChances <= 0.0f)
				return;

			foreach (var segmentConfig in _levelConfig.SegmentsChances)
			{
				_segmentsWeight.Add(new KeyValuePair<float, int>(segmentConfig.Chance / sumChances, segmentConfig.Length));
			}
		}

		public int GetNextSegmentLength()
		{
			float randomValue = Random.value;

			float curValue = 0.0f;
			foreach (var segmentConfig in _segmentsWeight)
			{
				curValue += segmentConfig.Key;
				if (curValue > randomValue)
					return segmentConfig.Value;
			}

			return 1;
		}

		private void AddNextSegment()
		{
			Vector2 direction = GetNextDirection();
			int length = GetNextSegmentLength();

			while (length > 0)
			{
				var tile = _actorsFactory.Create(ActorType.Tile);
				if (tile == null)
					return;
				tile.GameObject.transform.parent = Actor.GameObject.transform;
				var tileComponent = tile.GetSingleComponent<TileComponent>();
				if (tileComponent == null)
					return;
				tileComponent.Size.Value = _levelConfig.TileSize;
				tileComponent.Position.Value = _lastPosition;
				_tiles.Add(tileComponent);
				_lastPosition += direction;
				length--;
			}
		}

		private Vector2 GetNextDirection()
		{
			if (_lastDirection == Vector2.up)
			{
				_lastDirection = Vector2.right;
				return Vector2.up;
			}
			_lastDirection = Vector2.up;
			return Vector2.right;
		}

		private void CreatePath(int count)
		{
			for (int i = 0; i < count; i++)
			{
				AddNextSegment();
			}
		}
	}
}
