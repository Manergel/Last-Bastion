using UnityEngine;
using TMPro;

public class ResourceUI : MonoBehaviour
{
    public TextMeshProUGUI goldText;

    void Start()
    {
        ResourceManager.Instance.OnGoldChanged += UpdateUI;
        UpdateUI(); // показываем стартовое значение сразу
    }

    void OnDestroy()
    {
        ResourceManager.Instance.OnGoldChanged -= UpdateUI;
    }

    void UpdateUI()
    {
        goldText.text = $"Золото: {ResourceManager.Instance.Gold}";
    }
}