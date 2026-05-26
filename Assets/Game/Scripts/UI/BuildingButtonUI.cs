using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingButtonUI : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descText;
    public TextMeshProUGUI costText;
    public Button buildButton;

    private BuildingData building;

    public void Setup(BuildingData data)
    {
        building = data;
        nameText.text = building.buildingName;
        descText.text = building.description;
        costText.text = $"{building.cost}g";

        buildButton.onClick.AddListener(OnBuildClicked);

        // Подписываемся чтобы обновить кнопку после постройки
        BuildingSystem.Instance.OnBuildingsChanged += RefreshButton;
        ResourceManager.Instance.OnGoldChanged += RefreshButton;
        RefreshButton();
    }

    void OnDestroy()
    {
        BuildingSystem.Instance.OnBuildingsChanged -= RefreshButton;
        ResourceManager.Instance.OnGoldChanged -= RefreshButton;
    }

    void RefreshButton()
    {
        bool isBuilt = BuildingSystem.Instance.IsBuilt(building.buildingType);
        bool canAfford = ResourceManager.Instance.CanAfford(building.cost);

        buildButton.interactable = !isBuilt && canAfford;

        if (isBuilt)
            nameText.text = $"{building.buildingName} ✓";
    }

    void OnBuildClicked()
    {
        BuildingSystem.Instance.Build(building);
    }
}