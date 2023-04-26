using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECS;
using Game.ZigZag.Components;
using UnityEngine;
using UnityEngine.ProBuilder;

namespace Game.ZigZag.Systems
{
	[Serializable]
	public class TileSystem: BaseSystem, IAfterEntityInitialize, IAfterEntityDeInitialize
	{
		private TileComponent _tile;
		
		private ProBuilderMesh _mesh;

		public override void Initialize()
		{
		}

		public void AfterEntityInitialize()
		{
			_tile = Owner.GetSingleComponent<TileComponent>();
			if (_tile == null)
				return;

			_tile.Size.ValueChanged += TileSizeChanged;
			_tile.Position.ValueChanged += TilePositionChanged;

			RecreateTile();
		}
		public void AfterEntityDeInitialize()
		{
			if (_tile != null)
			{
				_tile.Size.ValueChanged -= TileSizeChanged;
				_tile.Position.ValueChanged -= TilePositionChanged;
				_tile = null;
			}

			ClearMesh();
		}

		private void UpdatePosition()
		{
			if (_tile == null)
				return;
			
			int size = _tile.Size.Value;
			Vector2 position = _tile.Position.Value;

			Actor.GameObject.transform.localPosition = new Vector3(position.x * size - size * 0.5f, 0.0f, position.y * size - size * 0.5f);
		}

		private void RecreateTile()
		{
			ClearMesh();

			_mesh = CreateMesh();

			UpdatePosition();
		}

		private ProBuilderMesh CreateMesh()
		{
			if (_tile == null)
				return null;

			int size = _tile.Size.Value;

			if (size <= 0)
				return null;

			var mesh = ShapeGenerator.GenerateCube(PivotLocation.Center, new Vector3(size, 1.0f, size));
			mesh.transform.parent = Actor.GameObject.transform;
			mesh.GetComponent<MeshRenderer>().sharedMaterial = _tile.Material != null ? _tile.Material : BuiltinMaterials.defaultMaterial;
			return mesh;
		}

		private void ClearMesh()
		{
			if (_mesh != null)
			{
				GameObject.Destroy(_mesh.gameObject);
				_mesh = null;
			}
		}

		private void TileSizeChanged(int oldSize, int newSize)
		{
			RecreateTile();
		}

		private void TilePositionChanged(Vector2 oldPos, Vector2 newPos)
		{
			UpdatePosition();
		}
	}
}
