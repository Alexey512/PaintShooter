using UnityEngine;

namespace Core.Paint
{
	public class PaintManager : MonoBehaviour, IPaintManager
	{
		[SerializeField]
		private Brush _defaultBrush = new Brush();
		
		private GameObject _brushObject;

		public Brush DefaultBrush => _defaultBrush;

		public void PaintObject(PaintTexture texture, Vector3 point, Vector3 normal, Brush brush)
		{
			if (texture == null)
				return;

			if (_brushObject == null)
			{
				_brushObject = new GameObject();
				_brushObject.name = "BrushObject";
				_brushObject.hideFlags = HideFlags.HideInHierarchy;
			}

			_brushObject.transform.position = point;

			Vector3 leftVec = Vector3.Cross(normal, Vector3.up);
			_brushObject.transform.rotation = leftVec.magnitude > 0.001f ? Quaternion.LookRotation(leftVec, normal) : Quaternion.identity;

			_brushObject.transform.RotateAround(point, normal, brush.Rotation);
			_brushObject.transform.localScale = Vector3.one * brush.Scale;

			BrushStroke newBrushStroke = new BrushStroke();
			newBrushStroke.Matrix = _brushObject.transform.worldToLocalMatrix;
			newBrushStroke.Channel = brush.GetMask();
			newBrushStroke.Scale = brush.GetTile();
			newBrushStroke.Brush = brush;

			texture.Paint(newBrushStroke);
		}

		public void PaintLine(Vector3 start, Vector3 end, Brush brush)
		{
			Ray ray = new Ray(start, (end - start).normalized);
			PaintRaycast(ray, brush);
		}

		public void PaintRay(Ray ray, Brush brush)
		{
			PaintRaycast(ray, brush);
		}

		public void PaintCollision(Collision collision, Brush brush)
		{
			foreach (ContactPoint contact in collision.contacts)
			{
				PaintTexture paintTexture = contact.otherCollider.GetComponent<PaintTexture>();
				if (paintTexture != null)
				{
					PaintObject(paintTexture, contact.point, contact.normal, brush);
				}
			}
		}

		public void Clear()
		{
			PaintTexture[] targets = GameObject.FindObjectsOfType<PaintTexture>() as PaintTexture[];

			foreach (PaintTexture target in targets)
			{
				target.ClearPaint();
			}
		}

		private void PaintRaycast(Ray ray, Brush brush, bool multi = true)
		{
			if (Physics.Raycast(ray, out var hit, 10000))
			{
				if (multi)
				{
					RaycastHit[] hits = Physics.SphereCastAll(hit.point, brush.Scale, ray.direction);
					for (int i = 0; i < hits.Length; i++)
					{
						PaintTexture paintTexture = hits[i].collider.gameObject.GetComponent<PaintTexture>();
						if (paintTexture != null)
						{
							PaintObject(paintTexture, hit.point, hits[i].normal, brush);
						}
					}
				}
				else
				{
					PaintTexture paintTexture = hit.collider.gameObject.GetComponent<PaintTexture>();
					if (!paintTexture) return;
					PaintObject(paintTexture, hit.point, hit.normal, brush);
				}
			}
		}

	}
}