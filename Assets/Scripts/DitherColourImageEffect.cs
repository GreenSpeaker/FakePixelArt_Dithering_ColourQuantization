using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

[ExecuteInEditMode]
public class DitherColourImageEffect : MonoBehaviour
{
    [Header("Materials")]
    [SerializeField] Material ditherMaterial;
    [SerializeField] Material paletteSwapMaterial;

    [Header("Downsample settings")]
    [SerializeField][Range(0, 10)] int downSampleAmount = 2;

    [Header("Dither and Colour Quantization Settings")]
    [SerializeField] [Range(2, 255)] int ColourRange = 2;
    [SerializeField] [Range(0.0f, 10.0f)] int DitherSpread = 1;

    [Header("General")]
    [SerializeField] private FilterMode filterMode;
    [SerializeField] private StatsDisplay statsDisplay;
    [SerializeField] private bool ShowDisplayBox = true;
    
    [Header("Apply Effects?")]
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

        UpdateDisplayText();
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

    void UpdateDisplayText()
    {
        if(!ShowDisplayBox)
        {
            statsDisplay.SetDisplayBoxActive(false);
            return;
        }
        statsDisplay.SetDisplayBoxActive(true);

        var availableColours =(int)Mathf.Pow(ApplyDithering ? ColourRange : 255, 3);
        availableColours = ApplyPaletteSwap ? 4 : availableColours;
        
        statsDisplay.UpdateText(availableColours, DitherSpread, downSampleAmount);
    }

    void OnValidate()
    {
        UpdateDisplayText();
    }
}
