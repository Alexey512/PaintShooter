using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "Test/Custom Render Pipeline Asset")]
public class CustomRenderPipelineAsset:  RenderPipelineAsset
{
	protected override RenderPipeline CreatePipeline()
	{
		return new CustomRenderPipeline();
	}
}
