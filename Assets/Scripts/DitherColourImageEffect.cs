using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

[ExecuteInEditMode]
public class DitherColourImageEffect : MonoBehaviour
{
    [SerializeField] Material ditherMaterial;

    [SerializeField] [Range(2, 255)] int ColourRange = 2;
    [SerializeField] [Range(0.0f, 10.0f)] int DitherSpread = 1;

    [SerializeField][Range(0, 10)] int downSampleAmount = 2;

    [SerializeField] private bool AddDither;

    [SerializeField] private FilterMode filterMode;

    [SerializeField] private StatsDisplay statsDisplay;


    [SerializeField] Material paletteSwapMaterial;
    [SerializeField] private bool ApplyPaletteSwap = false;

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        var width = source.width >> downSampleAmount;
        var height = source.height >> downSampleAmount;

        // source.filterMode = FilterMode.Point;
        var downscaleRenderTexture = RenderTexture.GetTemporary(width, height);
        var ditherRenderTexture = RenderTexture.GetTemporary(width, height);
        downscaleRenderTexture.filterMode = filterMode;
        ditherRenderTexture.filterMode = filterMode;
        // Apply downsample
        Graphics.Blit(source, downscaleRenderTexture);


        ditherMaterial.SetInt("_ColourRange", ColourRange);
        ditherMaterial.SetFloat("_Spread", DitherSpread);
        // Apply dither and colour quantization
        Graphics.Blit(downscaleRenderTexture, destination, ditherMaterial);
        return;
        // Apply palette swap
        Graphics.Blit(ditherRenderTexture, destination, paletteSwapMaterial);
        
        downscaleRenderTexture.Release();
        ditherRenderTexture.Release();

        statsDisplay.UpdateText((int)Mathf.Pow(ColourRange, 3), DitherSpread, downSampleAmount);
    }

    void OnValidate()
    {            
        statsDisplay.UpdateText((int)Mathf.Pow(!AddDither ? 255 : ColourRange, 3), DitherSpread, downSampleAmount);
    }
}
