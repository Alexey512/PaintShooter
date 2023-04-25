using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECS;
using UnityEngine;
using UnityEngine.ProBuilder;

namespace Game.ZigZag.Systems
{
	[Serializable]
	public class TileSystem: BaseSystem
	{
		private ProBuilderMesh _mesh;
		
		public override void Initialize()
		{
			_mesh = ShapeGenerator.GenerateCube(PivotLocation.Center, Vector3.one);
			_mesh.transform.parent = Actor.GameObject.transform;
		}
	}
}
