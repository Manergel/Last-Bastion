using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitCardUI : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public Button sendButton;

    private UnitInstance unit;

    // Вызывается из RosterUI когда создаётся карточка
    public void Setup(UnitInstance unitInstance)
    {
        unit = unitInstance;
        nameText.text = unit.unitName;

        // Привязываем кнопку к конкретному юниту этой карточки
        sendButton.onClick.AddListener(OnSendClicked);
    }

    void OnSendClicked()
    {
        ExpeditionManager.Instance.SendOnExpedition(unit);
    }
}