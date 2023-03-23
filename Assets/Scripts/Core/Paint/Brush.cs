using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.Paint
{
	public class BrushStroke
	{
		public Matrix4x4 Matrix;

		public Vector4 Channel;
		
		public Vector4 Scale;
		
		public Brush Brush;
	}

	[Serializable]
	public class Brush
	{
		public Texture2D Texture;

		public int X = 1;

		public int Y = 1;

		public int Index = -1;

		public float Scale = 1.0f;

		public float Rotation = 0f;

		public int Channel = 0;

		public Vector4 GetMask()
		{
			switch (Channel)
			{
				case 0:
					return new Vector4(1, 0, 0, 0);
				case 1:
					return new Vector4(0, 1, 0, 0);
				case 2:
					return new Vector4(0, 0, 1, 0);
				case 3:
					return new Vector4(0, 0, 0, 1);
			}

			return Vector4.zero;
		}

		public Vector4 GetTile()
		{
			float scaleX = 1.0f / X;
			float scaleY = 1.0f / Y;

			if (Index >= X * Y)
			{
				Index = 0;
			}

			if (Index < 0)
			{
				Index = Random.Range(0, X * Y);
			}

			float brushX = scaleX * (Index % X);
			float brushY = scaleY * (Index / X);

			return new Vector4(scaleX, scaleY, brushX, brushY);
		}

		public Brush Clone()
		{
			return new Brush
			{
				Texture = this.Texture,
				Channel = this.Channel,
				Index = this.Index,
				Rotation = this.Rotation,
				Scale = this.Scale,
				X = this.X,
				Y = this.Y
			};
		}
	}
}
