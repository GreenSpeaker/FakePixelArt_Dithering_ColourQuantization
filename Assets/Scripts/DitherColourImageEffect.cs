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

    [SerializeField] private bool ApplyDownsample = false;
    [SerializeField] private bool ApplyDithering = false;
    [SerializeField] private bool ApplyPaletteSwap = false;


    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        var width = source.width >> (ApplyDownsample ? downSampleAmount : 0);
        var height = source.height >> (ApplyDownsample ? downSampleAmount : 0);

        var temp = RenderTexture.GetTemporary(width, height);
        temp.filterMode = filterMode;

        // Apply downsample
        Graphics.Blit(source, temp);
        
        if(ApplyDithering)
        temp = ApplyDither(temp);

        if(ApplyPaletteSwap)
        temp = ApplyPalette(temp);

        Graphics.Blit(temp, destination);

        statsDisplay.UpdateText((int)Mathf.Pow(ColourRange, 3), DitherSpread, downSampleAmount);
    }

    RenderTexture ApplyDither(RenderTexture input)
    {
        var tempRenderTexture = RenderTexture.GetTemporary(input.width, input.height);
        tempRenderTexture.filterMode = filterMode;

        ditherMaterial.SetInt("_ColourRange", ColourRange);
        ditherMaterial.SetFloat("_Spread", DitherSpread);

        // Apply dither and colour quantization
        Graphics.Blit(input, tempRenderTexture, ditherMaterial);

        return tempRenderTexture;
    }

    RenderTexture ApplyPalette(RenderTexture input)
    {
        var tempRenderTexture = RenderTexture.GetTemporary(input.width, input.height);
        tempRenderTexture.filterMode = filterMode;

        // Apply dither and colour quantization
        Graphics.Blit(input, tempRenderTexture, paletteSwapMaterial);

        return tempRenderTexture;
    }

    void OnValidate()
    {            
        statsDisplay.UpdateText((int)Mathf.Pow(!AddDither ? 255 : ColourRange, 3), DitherSpread, downSampleAmount);
    }
}
