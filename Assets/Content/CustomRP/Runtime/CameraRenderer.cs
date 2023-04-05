using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraRenderer
{
	private ScriptableRenderContext _context;

    private Camera _camera;

    private const string _bufferName = "Render Camera Test 1";

    private CommandBuffer _buffer = new CommandBuffer { name = _bufferName };

    private CullingResults _cullingResults;

    private static ShaderTagId unlitShaderTagId = new ShaderTagId("SRPDefaultUnlit");

    public void Render(ScriptableRenderContext context, Camera camera)
	{
		_context = context;
		_camera = camera;

		if (!Cull())
            return;

		Setup();

		DrawVisibleGeometry();

		Submit();
	}

    private void Setup()
    {
	    _context.SetupCameraProperties(_camera);

	    _buffer.ClearRenderTarget(true, true, Color.clear);

	    _buffer.BeginSample(_bufferName);

        ExecuteBuffer();
    }

    private void DrawVisibleGeometry()
    {
	    var sortSettings = new SortingSettings(_camera)
	    {
            criteria = SortingCriteria.CommonOpaque
	    };
	    var drawSettings = new DrawingSettings(unlitShaderTagId, sortSettings);
		var filterSettings = new FilteringSettings(RenderQueueRange.all);

        _context.DrawRenderers(_cullingResults, ref drawSettings, ref filterSettings);

        _context.DrawSkybox(_camera);
    }

    private void Submit()
    {
	    _buffer.EndSample(_bufferName);
	    _context.Submit();
    }

    private void ExecuteBuffer()
    {
        _context.ExecuteCommandBuffer(_buffer);
        _buffer.Clear();
    }

    private bool Cull()
    {
	    if (_camera.TryGetCullingParameters(out ScriptableCullingParameters cull))
	    {
		    _cullingResults = _context.Cull(ref cull);
		    return true;
	    }
	    return false;
    }
}
