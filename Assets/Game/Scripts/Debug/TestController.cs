using UnityEngine;
using UnityEngine.UI;

public class TestController : MonoBehaviour
{
    private HireSystem hireSystem;
    private ExpeditionManager expeditionManager;
    private UnitInstance currentUnit;

    // Перетащи кнопки из Hierarchy в Inspector
    public Button hireButton;
    public Button sendButton;

    void Start()
    {
        hireSystem = GetComponent<HireSystem>();
        expeditionManager = GetComponent<ExpeditionManager>();

        // Вместо клавиш — нажатие кнопок на экране
        hireButton.onClick.AddListener(OnHireClicked);
        sendButton.onClick.AddListener(OnSendClicked);
    }

    void OnHireClicked()
    {
        currentUnit = hireSystem.HireUnit();
    }

    void OnSendClicked()
    {
        if (currentUnit != null)
            expeditionManager.SendOnExpedition(currentUnit);
        else
            Debug.Log("Сначала найми юнита");
    }
}