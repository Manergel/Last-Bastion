using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UnitCardUI : MonoBehaviour
{
    [Header("UI элементы")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI chanceText;
    public TextMeshProUGUI statusText;
    public Button sendButton;
    public Image cardBackground; // фон карточки для подсветки

    public Action OnCardClicked;
    
    private UnitInstance unit;
    private bool isSelected  = false;
    
    public void Setup(UnitInstance unitInstance)
    {
        unit = unitInstance;
        unit.OnStatsChanged += RefreshCard;
        RefreshCard();

        // Если есть кнопка отправки — старый режим
        if (sendButton != null)
            sendButton.onClick.AddListener(OnSendClicked);

        // Клик по всей карточке — новый режим выбора
        Button cardButton = GetComponent<Button>();
        if (cardButton != null)
            cardButton.onClick.AddListener(() => OnCardClicked?.Invoke());
    }
    
    public void SetSelected(bool selected)
    {
        isSelected = selected;

        if (cardBackground != null)
        {
            // Золотой если выбран, тёмный если нет
            cardBackground.color = selected
                ? new Color(0.94f, 0.65f, 0f, 0.4f)
                : new Color(0.1f, 0.1f, 0.1f, 0.8f);
        }
    }

    void RefreshCard()
    {
        if (unit == null) return;

        nameText.text = unit.unitName;
        nameText.text = $"{unit.unitName} [{unit.ClassName}]";

        if (unit.isFatigued)
            statusText.text = $"Устал ({unit.fatigueTurnsLeft})";
        else if (unit.currentMorale <= 0)
            statusText.text = "Сломлен";
        else
            statusText.text = $"{unit.ClassAbility} · Мораль: {unit.currentMorale}";

        int chance = ExpeditionManager.Instance.CalcSuccessChance(unit);
        chanceText.text = $"{chance}%";

        if (chance >= 70) chanceText.color = Color.green;
        else if (chance >= 40) chanceText.color = Color.yellow;
        else chanceText.color = Color.red;

        if (sendButton != null)
            sendButton.interactable = unit.CanGoOnExpedition();
    }

    void OnDestroy()
    {
        if (unit != null)
            unit.OnStatsChanged -= RefreshCard;
    }

    void OnSendClicked()
    {
        ExpeditionManager.Instance.SendOnExpedition(unit);
    }
}