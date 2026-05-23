using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitCardUI : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI chanceText; // ← новое поле
    public Button sendButton;

    private UnitInstance unit;

    public void Setup(UnitInstance unitInstance)
    {
        unit = unitInstance;
        nameText.text = unit.unitName;
        UpdateChance(); // показываем шанс сразу при создании карточки

        sendButton.onClick.AddListener(OnSendClicked);
    }

    void UpdateChance()
    {
        int chance = ExpeditionManager.Instance.CalcSuccessChance(unit);
        chanceText.text = $"{chance}%";

        // Цвет меняется в зависимости от шанса
        if (chance >= 70)
            chanceText.color = Color.green;
        else if (chance >= 40)
            chanceText.color = Color.yellow;
        else
            chanceText.color = Color.red;
    }

    void OnSendClicked()
    {
        ExpeditionManager.Instance.SendOnExpedition(unit);
    }
}