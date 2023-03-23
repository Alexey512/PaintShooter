using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Core.Paint
{
	public enum TextureSize
	{
		Texture64x64 = 64,
		Texture128x128 = 128,
		Texture256x256 = 256,
		Texture512x512 = 512,
		Texture1024x1024 = 1024,
		Texture2048x2048 = 2048,
		Texture4096x4096 = 4096
	}

	public class PaintTexture : MonoBehaviour
	{
		[SerializeField] 
		private TextureSize _paintTextureSize = TextureSize.Texture256x256;

		[SerializeField] 
		private TextureSize _renderTextureSize = TextureSize.Texture256x256;

		[SerializeField] 
		private bool _initOnStart = false;
		
		public bool IsValid => _isValid;

		private static string _renderCameraName = "RenderTextureCamera";
		
		private static int _maxStrokesPerFrame = 10;

		private Camera _renderCamera = null;

		private RenderTexture _strokeTexture;
		
		private RenderTexture _strokeTextureTmp;

		private RenderTexture _positionsTexture;
		
		private RenderTexture _positionsTextureTmp;
		
		private RenderTexture _tangentsTexture;
		
		private RenderTexture _binormalsTexture;

		private Material _paintMaterial;
		
		private Material _positionsMaterial;
		
		private Material _tangentsMaterial;
		
		private Material _binormalsMaterial;

		private List<BrushStroke> _strokes = new List<BrushStroke>();

		private bool _isPrevFrame = false;
		
		private bool _isInitialize = false;

		private Renderer _paintRenderer;
		
		private bool _isValid = true;

		public void Paint(BrushStroke stroke)
		{
			_strokes.Add(stroke);
		}

		public void ClearPaint()
		{
			if (_isInitialize)
			{
				CommandBuffer cb = new CommandBuffer();
				cb.SetRenderTarget(_strokeTexture);
				cb.ClearRenderTarget(true, true, new Color(0, 0, 0, 0));
				cb.SetRenderTarget(_strokeTextureTmp);
				cb.ClearRenderTarget(true, true, new Color(0, 0, 0, 0));
				_renderCamera.AddCommandBuffer(CameraEvent.AfterEverything, cb);
				_renderCamera.Render();
				_renderCamera.RemoveAllCommandBuffers();
			}
		}

		private void Start()
		{
			CheckValid();
			
			if (_initOnStart)
			{
				Initialize();
			}
		}

		private void CheckValid()
		{
			_paintRenderer = this.GetComponent<Renderer>();
			if (_paintRenderer)
			{
				_isValid = true;
			}
		}
		
		private void Initialize()
		{
			InitializeRenderCamera();
			InitializeMaterials();
			InitializeTextures();

			RenderTextures();
			_isInitialize = true;
		}

		private void InitializeRenderCamera()
		{
			GameObject cameraObject = GameObject.Find(_renderCameraName);
			if (cameraObject != null)
			{
				_renderCamera = cameraObject.GetComponent<Camera>();
				return;
			}

			cameraObject = new GameObject
			{
				name = _renderCameraName,
				transform =
				{
					position = Vector3.zero,
					rotation = Quaternion.identity,
					localScale = Vector3.one
				},
				hideFlags = HideFlags.HideInHierarchy
			};

			_renderCamera = cameraObject.AddComponent<Camera>();
			_renderCamera.clearFlags = CameraClearFlags.SolidColor;
			_renderCamera.backgroundColor = new Color(0, 0, 0, 0);
			_renderCamera.orthographic = true;
			_renderCamera.nearClipPlane = 0.0f;
			_renderCamera.farClipPlane = 1.0f;
			_renderCamera.orthographicSize = 1.0f;
			_renderCamera.aspect = 1.0f;
			_renderCamera.useOcclusionCulling = false;
			_renderCamera.enabled = false;
			_renderCamera.cullingMask = LayerMask.NameToLayer("Nothing");
		}

		private void InitializeMaterials()
		{
			_paintMaterial = new Material(Shader.Find("Hidden/PaintBrush"));
			_positionsMaterial = new Material(Shader.Find("Hidden/PaintPosition"));
			_tangentsMaterial = new Material(Shader.Find("Hidden/PaintTangent"));
			_binormalsMaterial = new Material(Shader.Find("Hidden/PaintNormal"));
		}

		private void InitializeTextures()
		{
			_strokeTexture = CreateTenderTexture(_paintTextureSize, RenderTextureFormat.ARGB32);
			_strokeTextureTmp = CreateTenderTexture(_paintTextureSize, RenderTextureFormat.ARGB32);

			_positionsTexture = CreateTenderTexture(_renderTextureSize, RenderTextureFormat.ARGBFloat);
			_positionsTextureTmp = CreateTenderTexture(_renderTextureSize, RenderTextureFormat.ARGBFloat);
			
			_tangentsTexture = CreateTenderTexture(_renderTextureSize, RenderTextureFormat.ARGB32);
			_binormalsTexture = CreateTenderTexture(_renderTextureSize, RenderTextureFormat.ARGB32);

			foreach (Material mat in _paintRenderer.materials)
			{
				mat.SetTexture("_StrokeTex", _strokeTexture);
				mat.SetTexture("_WorldPosTex", _positionsTexture);
				mat.SetTexture("_WorldTangentTex", _tangentsTexture);
				mat.SetTexture("_WorldBinormalTex", _binormalsTexture);
			}
		}

		private RenderTexture CreateTenderTexture(TextureSize size, RenderTextureFormat format)
		{
			var renderTexture = new RenderTexture((int)size, (int)size, 0, format, RenderTextureReadWrite.Linear);
			renderTexture.Create();
			return renderTexture;
		}

		private void RenderTextures()
		{
			this.transform.hasChanged = false;

			CommandBuffer cb = new CommandBuffer();

			DrawTexture(cb, _positionsTexture, _positionsMaterial);
			DrawTexture(cb, _tangentsTexture, _tangentsMaterial);
			DrawTexture(cb, _binormalsTexture, _binormalsMaterial);

			_renderCamera.AddCommandBuffer(CameraEvent.AfterEverything, cb);
			_renderCamera.Render();
			_renderCamera.RemoveAllCommandBuffers();

			Graphics.Blit(_positionsTexture, _positionsTextureTmp, _paintMaterial, 2);
			Graphics.Blit(_positionsTextureTmp, _positionsTexture, _paintMaterial, 2);
		}

		private void DrawTexture(CommandBuffer buffer, RenderTexture texture, Material material)
		{
			buffer.SetRenderTarget(texture);
			buffer.ClearRenderTarget(true, true, new Color(0, 0, 0, 0));
			for (int i = 0; i < _paintRenderer.materials.Length; i++)
			{
				buffer.DrawRenderer(_paintRenderer, material, i);
			}
		}

		private void PaintStrokes()
		{
			if (!_isValid)
				return;
			
			if (_strokes.Count > 0)
			{
				if (!_isInitialize)
				{
					Initialize();
				}

				if (transform.hasChanged)
				{
					RenderTextures();
				}

				Matrix4x4[] strokeMatrix = new Matrix4x4[10];
				Vector4[] strokeScale = new Vector4[10];
				Vector4[] strokeChannel = new Vector4[10];

				int i = 0;
				Texture2D strokeTexture = _strokes[0].Brush.Texture;

				for (int index = 0; index < _strokes.Count;)
				{
					if (i >= _maxStrokesPerFrame) 
						break;

					if (_strokes[index].Brush.Texture == strokeTexture)
					{
						strokeMatrix[i] = _strokes[index].Matrix;
						strokeScale[i] = _strokes[index].Scale;
						strokeChannel[i] = _strokes[index].Channel;
						_strokes.RemoveAt(index);
						i++;
					}
					else
					{
						index++;
					}
				}

				_paintMaterial.SetMatrixArray("_StrokeMatrix", strokeMatrix);
				_paintMaterial.SetVectorArray("_StrokeScale", strokeScale);
				_paintMaterial.SetVectorArray("_StrokeChannel", strokeChannel);

				_paintMaterial.SetInt("_TotalStrokes", i);

				_paintMaterial.SetTexture("_WorldPosTex", _positionsTexture);

				//TODO: Refactor
				if (_isPrevFrame)
				{
					_paintMaterial.SetTexture("_LastStrokeTex", _strokeTextureTmp);
					Graphics.Blit(strokeTexture, _strokeTexture, _paintMaterial, 0);

					foreach (Material mat in _paintRenderer.materials)
					{
						mat.SetTexture("_StrokeTex", _strokeTexture);
					}

					_isPrevFrame = false;
				}
				else
				{
					_paintMaterial.SetTexture("_LastStrokeTex", _strokeTexture);
					Graphics.Blit(strokeTexture, _strokeTextureTmp, _paintMaterial, 0);
					foreach (Material mat in _paintRenderer.materials)
					{
						mat.SetTexture("_StrokeTex", _strokeTextureTmp);
					}

					_isPrevFrame = true;
				}
			}
		}

		private void Update()
		{
			PaintStrokes();
		}
	}
}