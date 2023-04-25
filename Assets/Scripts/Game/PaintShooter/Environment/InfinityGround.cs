using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.PaintShooter.Environment
{
	public class InfinityGround: MonoBehaviour
	{
		[SerializeField]
		private Transform _unit;

		[SerializeField]
		private BoxCollider _groudPrefab;

		[SerializeField]
		private Vector3 _moveAxis;

		private BoxCollider _currentChunk;

		private BoxCollider _nextChunk;

		private BoxCollider _prevChunk;

		private Stack<BoxCollider> _chunksPull = new Stack<BoxCollider>();

		private void Start()
		{
			_groudPrefab?.gameObject.SetActive(false);
			UpdateChunk();
		}

		private void UpdateChunk()
		{
			if (_unit == null || _groudPrefab == null)
				return;

			if (_currentChunk == null)
			{
				_currentChunk = CreateChunk(Vector3.zero);
			}

			Vector3 unitPosition = _unit.transform.position;
			Vector3 chunkCenter = _currentChunk.transform.position + _currentChunk.center;
			Vector3 chunkSize = _currentChunk.size;

			if (unitPosition.x > chunkCenter.x && _nextChunk == null)
			{
				var nextChunkCenter = chunkCenter + new Vector3(chunkSize.x, 0.0f, 0.0f) - _currentChunk.center;
				_nextChunk = CreateChunk(nextChunkCenter);
			}

			if (unitPosition.x > chunkCenter.x + chunkSize.x * 0.5f && _nextChunk != null)
			{
				_prevChunk = _currentChunk;
				_currentChunk = _nextChunk;
				_nextChunk = null;
			}

			if (_prevChunk != null)
			{
				Vector3 prevChunkCenter = _prevChunk.transform.position + _prevChunk.center;
				if (unitPosition.x > prevChunkCenter.x + chunkSize.x)
				{
					ReleaseChunk(_prevChunk);
					_prevChunk = null;
				}
			}
		}

		private BoxCollider CreateChunk(Vector3 position)
		{
			BoxCollider chunk = _chunksPull.Count > 0 ? _chunksPull.Pop() : Instantiate(_groudPrefab.gameObject, transform).GetComponent<BoxCollider>();
			chunk.gameObject.SetActive(true);
			chunk.transform.position = position;
			return chunk;
		}

		private void ReleaseChunk(BoxCollider chunk)
		{
			chunk.gameObject.SetActive(false);
			_chunksPull.Push(chunk);
		}

		private void Update()
		{
			UpdateChunk();
		}
	}
}
