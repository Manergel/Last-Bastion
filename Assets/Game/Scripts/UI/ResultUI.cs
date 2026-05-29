using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultUI : MonoBehaviour
{
    public static ResultUI Instance { get; private set; }

    [Header("Текст")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI goldText;

    [Header("Кнопки")]
    public Button returnToBaseButton;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void Show(NodeResult result)
    {
        titleText.text = result.success ? "Успех" : "Провал";
        resultText.text = result.resultText;
        goldText.text = result.goldEarned > 0
            ? $"+{result.goldEarned} золота"
            : "Золото не получено";

        if (result.deadUnit != null)
            resultText.text += $"\n\n{result.deadUnit.unitName} погиб навсегда.";

        returnToBaseButton.onClick.RemoveAllListeners();
        returnToBaseButton.onClick.AddListener(OnReturnToBase);
    }

    void OnReturnToBase()
    {
        UIManager.Instance.ShowPanel(UIManager.Instance.unitSelectionPanel);
    }
}