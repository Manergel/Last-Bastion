using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExpeditionUI : MonoBehaviour
{
    [Header("Текст события")]
    public TextMeshProUGUI nodeCounterText;
    public TextMeshProUGUI eventTitleText;
    public TextMeshProUGUI eventDescriptionText;
    public TextMeshProUGUI chanceText;

    [Header("Кнопки выбора")]
    public Button choice1Button;
    public Button choice2Button;
    public TextMeshProUGUI choice1Text;
    public TextMeshProUGUI choice2Text;

    void OnEnable()
    {
        // Вызывается когда панель включается — показываем текущий узел
        ShowCurrentNode();
    }

    void ShowCurrentNode()
    {
        ExpeditionNode node = ExpeditionManager.Instance.GetCurrentNode();
        if (node == null) return;

        EventData e = node.eventData;

        // Заполняем текст
        nodeCounterText.text = $"Узел {ExpeditionManager.Instance.CurrentNodeIndex + 1} из {ExpeditionManager.Instance.TotalNodes}";
        eventTitleText.text = e.eventTitle;
        eventDescriptionText.text = e.eventDescription;

        // Заполняем кнопки
        choice1Text.text = e.choices[0].choiceText;
        choice2Text.text = e.choices[1].choiceText;

        // Показываем шанс первого выбора по умолчанию
        UpdateChanceText(0);

        // Вешаем обработчики
        choice1Button.onClick.RemoveAllListeners();
        choice2Button.onClick.RemoveAllListeners();

        choice1Button.onClick.AddListener(() => OnChoiceSelected(0));
        choice2Button.onClick.AddListener(() => OnChoiceSelected(1));
    }

    void UpdateChanceText(int choiceIndex)
    {
        int chance = ExpeditionManager.Instance.currentSquad.Count > 0
            ? CalculateChance(choiceIndex)
            : 0;

        chanceText.text = $"Шанс успеха: {chance}%";

        if (chance >= 70) chanceText.color = Color.green;
        else if (chance >= 40) chanceText.color = Color.yellow;
        else chanceText.color = Color.red;
    }

    int CalculateChance(int choiceIndex)
    {
        EventData e = ExpeditionManager.Instance.GetCurrentNode().eventData;
        int chance = e.choices[choiceIndex].baseSuccessChance;

        // Бонус/штраф от морали отряда
        int avgMorale = 0;
        foreach (UnitInstance unit in ExpeditionManager.Instance.currentSquad)
            avgMorale += unit.currentMorale;
        avgMorale /= ExpeditionManager.Instance.currentSquad.Count;

        if (avgMorale < 50) chance -= 15;
        if (avgMorale > 80) chance += 10;

        return Mathf.Clamp(chance, 5, 95);
    }

    void OnChoiceSelected(int choiceIndex)
    {
        // Обновляем шанс для выбранного варианта
        UpdateChanceText(choiceIndex);

        // Разрешаем узел
        NodeResult result = ExpeditionManager.Instance.ResolveCurrentNode(choiceIndex);
        if (result == null) return;

        // Показываем экстракшн или результат
        if (ExpeditionManager.Instance.IsLastNode)
        {
            // Последний узел — сразу результат
            ExpeditionManager.Instance.EndExpedition();
            ShowResult(result);
        }
        else
        {
            // Показываем экстракшн — уйти или продолжить
            ExtractionUI.Instance.Show(result);
            UIManager.Instance.ShowPanel(UIManager.Instance.extractionPanel);
        }
    }

    void ShowResult(NodeResult result)
    {
        ResultUI.Instance.Show(result);
        UIManager.Instance.ShowPanel(UIManager.Instance.resultPanel);
    }
}