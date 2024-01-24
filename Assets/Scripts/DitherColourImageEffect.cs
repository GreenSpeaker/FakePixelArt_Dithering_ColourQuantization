using UnityEngine;

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


    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        var width = source.width >> downSampleAmount;
        var height = source.height >> downSampleAmount;

        // source.filterMode = FilterMode.Point;
        var tempTexture = RenderTexture.GetTemporary(width, height);
        tempTexture.filterMode = filterMode;
        Graphics.Blit(source, tempTexture);

        if(!AddDither)
        {
            statsDisplay.UpdateText((int)Mathf.Pow(255, 3), DitherSpread, downSampleAmount);
            Graphics.Blit(tempTexture, destination);
            return;
        }

        ditherMaterial.SetInt("_ColourRange", ColourRange);
        ditherMaterial.SetFloat("_Spread", DitherSpread);
        Graphics.Blit(tempTexture, destination, ditherMaterial);

        tempTexture.Release();

        statsDisplay.UpdateText((int)Mathf.Pow(ColourRange, 3), DitherSpread, downSampleAmount);
    }

    void OnValidate()
    {            
        statsDisplay.UpdateText((int)Mathf.Pow(!AddDither ? 255 : ColourRange, 3), DitherSpread, downSampleAmount);
    }
}
