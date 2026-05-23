using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitCardUI : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI chanceText;
    public TextMeshProUGUI statusText;

    public Button sendButton;

    private UnitInstance unit;

    public void Setup(UnitInstance unitInstance)
    {
        unit = unitInstance;

        // Подписываемся — карточка обновится когда изменятся stats
        unit.OnStatsChanged += RefreshCard;

        RefreshCard();
        sendButton.onClick.AddListener(OnSendClicked);
    }
    
    void OnDestroy()
    {
        if (unit != null)
            unit.OnStatsChanged -= RefreshCard;
    }

    void RefreshCard()
    {
        nameText.text = unit.unitName;

        // Статус
        if (unit.isFatigued)
            statusText.text = $"Устал ({unit.fatigueTurnsLeft})";
        else if (unit.currentMorale <= 0)
            statusText.text = "Сломлен";
        else
            statusText.text = $"Мораль: {unit.currentMorale}";

        // Шанс
        int chance = ExpeditionManager.Instance.CalcSuccessChance(unit);
        chanceText.text = $"{chance}%";

        if (chance >= 70) chanceText.color = Color.green;
        else if (chance >= 40) chanceText.color = Color.yellow;
        else chanceText.color = Color.red;

        // Блокируем кнопку если нельзя идти
        sendButton.interactable = unit.CanGoOnExpedition();
    }

    void OnSendClicked()
    {
        ExpeditionManager.Instance.SendOnExpedition(unit);
    }
}