using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECS;
using Game.ZigZag.Components;
using Game.ZigZag.Events;
using Game.ZigZag.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Game.ZigZag.Systems
{
	[Serializable]
	public class MoveSystem: BaseSystem, IAfterEntityInitialize, IAfterEntityDeInitialize
	{
		[Inject]
		private SignalBus _signalBus;
		
		private PlayerComponent _player;

		private Vector3 _direction = Vector3.zero;

		public void AfterEntityInitialize()
		{
			_player = EntityManager.Instance.GetComponents<PlayerComponent>().FirstOrDefault();
			if (_player == null)
				return;

			_signalBus.Subscribe<TapScreenEvent>(OnTap);
		}

		public void AfterEntityDeInitialize()
		{
			_signalBus.Unsubscribe<TapScreenEvent>(OnTap);
		}

		public override void Update()
		{
			if (_player == null)
				return;

			_player.Actor.GameObject.transform.position += _direction * Time.deltaTime * _player.Speed;
		}

		private void OnTap()
		{
			if (_direction == Vector3.zero)
			{
				_direction = Vector3.forward;
			}
			else if (_direction == Vector3.forward)
			{
				_direction = Vector3.right;
			}
			else if (_direction == Vector3.right)
			{
				_direction = Vector3.forward;
			}
			else
			{
				_direction = Vector3.forward;
			}
		}
	}
}
