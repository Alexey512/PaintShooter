using System.Collections.Generic;
using Core.Utility;
using Core.ECS;
using UnityEngine;

namespace Game.Core.ECS
{
	[CreateAssetMenu(fileName = "EntityConfigAsset", menuName = "Game/Entity Config", order = -1)]
	public class EntityConfigAsset: ScriptableObject
    {
		[SerializeField]
        private EntityConfig _config;

        public EntityConfig Config => _config;

        public void Initialize(IEntity entity)
		{
			if (_config == null || entity == null)
				return;

			entity.Clear();

            _config.Initialize(entity);
		}
	}
}
