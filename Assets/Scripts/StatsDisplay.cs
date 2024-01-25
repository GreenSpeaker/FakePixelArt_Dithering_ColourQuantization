
using TMPro;
using UnityEngine;

public class StatsDisplay : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI text;

    // Update is called once per frame
    public void UpdateText(int coloursAvailable, int ditherSpread, int downSampleAmount)
    {
        text.text = $"Colours on screen:\n4\n\nDither Spread:\n{ditherSpread}\n\nDownSampling:\n{downSampleAmount}";
    }
}
