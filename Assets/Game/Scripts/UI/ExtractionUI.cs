using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExtractionUI : MonoBehaviour
{
    public static ExtractionUI Instance { get; private set; }

    [Header("Текст")]
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI squadStatusText;

    [Header("Кнопки")]
    public Button retreatButton;
    public Button continueButton;

    private NodeResult lastResult;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void Show(NodeResult result)
    {
        lastResult = result;

        // Показываем что произошло
        resultText.text = result.resultText;

        // Показываем состояние отряда
        string status = "";
        foreach (UnitInstance unit in ExpeditionManager.Instance.currentSquad)
        {
            string unitStatus = unit.isFatigued ? "устал" :
                unit.currentMorale < 30 ? "сломлен" : "в порядке";
            status += $"{unit.unitName}: {unitStatus}\n";
        }
        squadStatusText.text = status;

        retreatButton.onClick.RemoveAllListeners();
        continueButton.onClick.RemoveAllListeners();

        retreatButton.onClick.AddListener(OnRetreat);
        continueButton.onClick.AddListener(OnContinue);
    }

    void OnRetreat()
    {
        // Уходим с добычей
        ExpeditionManager.Instance.EndExpedition();
        ResultUI.Instance.Show(lastResult);
        UIManager.Instance.ShowPanel(UIManager.Instance.resultPanel);
    }

    void OnContinue()
    {
        // Идём дальше
        ExpeditionManager.Instance.AdvanceToNextNode();
        UIManager.Instance.ShowPanel(UIManager.Instance.expeditionPanel);
    }
}