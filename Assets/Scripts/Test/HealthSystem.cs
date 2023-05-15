using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.PaintShooter.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Test
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(HealthSystem))]	
	public sealed class HealthSystem : UpdateSystem 
	{
		private Filter filter;
		private Stash<HealthComponent> healthStash;
    
		public override void OnAwake() {
			this.filter = this.World.Filter.With<HealthComponent>();
			this.healthStash = this.World.GetStash<HealthComponent>();
		}

		public override void OnUpdate(float deltaTime) {
			foreach (var entity in this.filter) {
				ref var healthComponent = ref healthStash.Get(entity);
				Debug.Log(healthComponent.healthPoints);
			}
		}
	}
}
