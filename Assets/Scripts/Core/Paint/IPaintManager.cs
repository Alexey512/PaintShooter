using UnityEngine;

namespace Core.Paint
{
	public interface IPaintManager
	{
		Brush DefaultBrush { get; }

		void PaintObject(PaintTexture texture, Vector3 point, Vector3 normal, Brush brush);

		void PaintLine(Vector3 start, Vector3 end, Brush brush);

		void PaintRay(Ray ray, Brush brush);

		void PaintCollision(Collision collision, Brush brush);

		void Clear();
	}
}
