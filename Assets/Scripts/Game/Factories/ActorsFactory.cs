using Core.Factories;
using UnityEngine;
using Zenject;

namespace Game.Factories
{
	[CreateAssetMenu(fileName = "ActorsFactory", menuName = "Game/Actors Factory", order = -1)]
	public class ActorsFactory: GameObjectsFactory
	{
		[Inject]
		private void Construct(DiContainer container)
		{
			Initialize(container);
		}
	}
}
