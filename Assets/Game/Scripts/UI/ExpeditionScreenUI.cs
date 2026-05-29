using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExpeditionScreenUI : MonoBehaviour
{
    public static ExpeditionScreenUI Instance { get; private set; }

    [Header("Панели внутри экспедиции")]
    public GameObject nodePanel;        // выбор действия
    public GameObject extractionPanel;  // уйти / продолжить
    public GameObject resultPanel;      // итог экспедиции

    [Header("Node Panel")]
    public TextMeshProUGUI nodeCounterText;
    public TextMeshProUGUI eventTitleText;
    public TextMeshProUGUI eventDescriptionText;
    public TextMeshProUGUI chanceText;
    public Button choice1Button;
    public Button choice2Button;
    public TextMeshProUGUI choice1Text;
    public TextMeshProUGUI choice2Text;

    [Header("Extraction Panel")]
    public TextMeshProUGUI extractionResultText;
    public TextMeshProUGUI squadStatusText;
    public Button retreatButton;
    public Button continueButton;

    [Header("Result Panel")]
    public TextMeshProUGUI resultTitleText;
    public TextMeshProUGUI resultDescText;
    public TextMeshProUGUI goldText;
    public Button returnButton;

    private NodeResult lastResult;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    void OnEnable()
    {
        ShowNodePanel();
    }

    // ─── NODE ───────────────────────────────

    void ShowNodePanel()
    {
        nodePanel.SetActive(true);
        extractionPanel.SetActive(false);
        resultPanel.SetActive(false);

        ExpeditionNode node = ExpeditionManager.Instance.GetCurrentNode();
        if (node == null)
        {
            Debug.LogError("Нет текущего узла — экспедиция не началась?");
            return;
        }

        EventData e = node.eventData;

        nodeCounterText.text = $"Узел {ExpeditionManager.Instance.CurrentNodeIndex + 1} из {ExpeditionManager.Instance.TotalNodes}";
        eventTitleText.text = e.eventTitle;
        eventDescriptionText.text = e.eventDescription;
        choice1Text.text = e.choices[0].choiceText;
        choice2Text.text = e.choices[1].choiceText;

        UpdateChance(0);

        choice1Button.onClick.RemoveAllListeners();
        choice2Button.onClick.RemoveAllListeners();
        choice1Button.onClick.AddListener(() => OnChoiceSelected(0));
        choice2Button.onClick.AddListener(() => OnChoiceSelected(1));
    }

    void UpdateChance(int choiceIndex)
    {
        EventData e = ExpeditionManager.Instance.GetCurrentNode().eventData;
        int chance = e.choices[choiceIndex].baseSuccessChance;

        int avgMorale = 0;
        foreach (UnitInstance unit in ExpeditionManager.Instance.currentSquad)
            avgMorale += unit.currentMorale;
        if (ExpeditionManager.Instance.currentSquad.Count > 0)
            avgMorale /= ExpeditionManager.Instance.currentSquad.Count;

        if (avgMorale < 50) chance -= 15;
        if (avgMorale > 80) chance += 10;
        chance = Mathf.Clamp(chance, 5, 95);

        chanceText.text = $"Шанс успеха: {chance}%";
        chanceText.color = chance >= 70 ? Color.green :
                           chance >= 40 ? Color.yellow : Color.red;
    }

    void OnChoiceSelected(int choiceIndex)
    {
        lastResult = ExpeditionManager.Instance.ResolveCurrentNode(choiceIndex);
        if (lastResult == null) return;

        if (ExpeditionManager.Instance.IsLastNode)
        {
            ExpeditionManager.Instance.EndExpedition();
            ShowResultPanel();
        }
        else
        {
            ShowExtractionPanel();
        }
    }

    // ─── EXTRACTION ─────────────────────────

    void ShowExtractionPanel()
    {
        nodePanel.SetActive(false);
        extractionPanel.SetActive(true);
        resultPanel.SetActive(false);

        extractionResultText.text = lastResult.resultText;

        string status = "";
        foreach (UnitInstance unit in ExpeditionManager.Instance.currentSquad)
        {
            string s = unit.isFatigued ? "устал" :
                       unit.currentMorale < 30 ? "сломлен" : "в порядке";
            status += $"{unit.unitName}: {s}\n";
        }
        squadStatusText.text = status;

        retreatButton.onClick.RemoveAllListeners();
        continueButton.onClick.RemoveAllListeners();
        retreatButton.onClick.AddListener(OnRetreat);
        continueButton.onClick.AddListener(OnContinue);
    }

    void OnRetreat()
    {
        ExpeditionManager.Instance.EndExpedition();
        ShowResultPanel();
    }

    void OnContinue()
    {
        ExpeditionManager.Instance.AdvanceToNextNode();
        ShowNodePanel();
    }

    // ─── RESULT ─────────────────────────────

    void ShowResultPanel()
    {
        nodePanel.SetActive(false);
        extractionPanel.SetActive(false);
        resultPanel.SetActive(true);

        resultTitleText.text = lastResult.success ? "Успех" : "Провал";
        resultDescText.text = lastResult.resultText;
        goldText.text = lastResult.goldEarned > 0
            ? $"+{lastResult.goldEarned} золота"
            : "Золото не получено";

        if (lastResult.deadUnit != null)
            resultDescText.text += $"\n\n{lastResult.deadUnit.unitName} погиб навсегда.";

        returnButton.onClick.RemoveAllListeners();
        returnButton.onClick.AddListener(OnReturnToBase);
    }

    void OnReturnToBase()
    {
        UIManager.Instance.ShowPanel(UIManager.Instance.unitSelectionPanel);
    }
}