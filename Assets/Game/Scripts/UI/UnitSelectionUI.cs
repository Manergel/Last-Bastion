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
        startExpeditionButton.onClick.AddListener(OnStartExpedition);
        startExpeditionButton.interactable = false;

        // Подписываемся — карточки появятся когда юниты будут созданы
        RosterManager.Instance.OnRosterChanged += SpawnUnitCards;
    
        // На случай если юниты уже созданы
        if (RosterManager.Instance.GetUnits().Count > 0)
            SpawnUnitCards();
    }

    void OnDestroy()
    {
        RosterManager.Instance.OnRosterChanged -= SpawnUnitCards;
    }

    void SpawnUnitCards()
    {
        // Очищаем старые карточки перед пересозданием
        foreach (Transform child in unitsContainer)
            Destroy(child.gameObject);
        spawnedCards.Clear();
        selectedUnits.Clear();

        Debug.Log($"Юнитов в ростере: {RosterManager.Instance.GetUnits().Count}");

        foreach (UnitInstance unit in RosterManager.Instance.GetUnits())
        {
            UnitCardUI card = Instantiate(cardPrefab, unitsContainer);
            Debug.Log($"Карточка создана: {card.name}, родитель: {card.transform.parent?.name}");
            card.Setup(unit);
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