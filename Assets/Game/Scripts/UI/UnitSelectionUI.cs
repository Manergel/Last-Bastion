using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSelectionUI : MonoBehaviour
{
    [Header("Ссылки")]
    public Transform unitsContainer;
    public UnitCardUI cardPrefab;
    public Button startExpeditionButton;

    private List<UnitInstance> selectedUnits = new List<UnitInstance>();
    private List<UnitCardUI> spawnedCards = new List<UnitCardUI>();

    private const int MAX_SELECTED = 3;

    void Start()
    {
        SpawnUnitCards();
        startExpeditionButton.onClick.AddListener(OnStartExpedition);
        startExpeditionButton.interactable = false; // нельзя начать без отряда
    }

    void SpawnUnitCards()
    {
        // Создаём карточки для всех доступных юнитов
        foreach (UnitInstance unit in RosterManager.Instance.GetUnits())
        {
            UnitCardUI card = Instantiate(cardPrefab, unitsContainer);
            card.Setup(unit);

            // Передаём колбэк — карточка сообщит когда её выбрали
            card.OnCardClicked = () => OnUnitSelected(unit, card);
            spawnedCards.Add(card);
        }
    }

    void OnUnitSelected(UnitInstance unit, UnitCardUI card)
    {
        if (selectedUnits.Contains(unit))
        {
            // Снимаем выбор
            selectedUnits.Remove(unit);
            card.SetSelected(false);
        }
        else if (selectedUnits.Count < MAX_SELECTED)
        {
            // Выбираем
            selectedUnits.Add(unit);
            card.SetSelected(true);
        }

        // Кнопка активна только если выбран хотя бы один
        startExpeditionButton.interactable = selectedUnits.Count > 0;
    }

    void OnStartExpedition()
    {
        if (selectedUnits.Count == 0) return;

        // Передаём выбранный отряд в ExpeditionManager и переключаем панель
        ExpeditionManager.Instance.StartExpedition(selectedUnits);
        UIManager.Instance.ShowPanel(UIManager.Instance.expeditionPanel);
    }
}