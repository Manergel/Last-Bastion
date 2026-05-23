using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CorruptionUI : MonoBehaviour
{
    public TextMeshProUGUI[] regionTexts; // три текста — по одному на регион

    void Start()
    {
        CorruptionSystem.Instance.OnCorruptionChanged += RefreshUI;
        TurnManager.Instance.OnTurnEnded += RefreshUI;
        RefreshUI();
    }

    void OnDestroy()
    {
        CorruptionSystem.Instance.OnCorruptionChanged -= RefreshUI;
        TurnManager.Instance.OnTurnEnded -= RefreshUI;
    }

    void RefreshUI()
    {
        RegionData[] regions = CorruptionSystem.Instance.regions;

        for (int i = 0; i < regionTexts.Length; i++)
        {
            if (i >= regions.Length) break;

            RegionData r = regions[i];

            if (r.isConquered)
            {
                regionTexts[i].text = $"{r.regionName}: ЗАХВАЧЕН";
                regionTexts[i].color = Color.red;
            }
            else
            {
                regionTexts[i].text = $"{r.regionName}: {r.currentCorruption}/3";

                if (r.currentCorruption == 0)
                    regionTexts[i].color = Color.green;
                else if (r.currentCorruption == 1)
                    regionTexts[i].color = Color.yellow;
                else
                    regionTexts[i].color = new Color(1f, 0.5f, 0f); // оранжевый
            }
        }
    }
}