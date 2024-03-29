using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsDisplay : MonoBehaviour
{
    [SerializeField] private Image backing;
    [SerializeField] private TextMeshProUGUI text;

    // Update is called once per frame
    public void UpdateText(int coloursAvailable, int ditherSpread, int downSampleAmount)
    {
        text.text = $"Colours on screen:\n{coloursAvailable}\n\nDither Spread:\n{ditherSpread}\n\nDownSampling:\n{downSampleAmount}";
    }

    public void SetDisplayBoxActive(bool active)
    {
        backing.gameObject.SetActive(active);
    }
}
