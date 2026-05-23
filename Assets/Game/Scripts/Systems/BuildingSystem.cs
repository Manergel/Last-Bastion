using System.Collections.Generic;
using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem Instance { get; private set; }

    public BuildingData[] availableBuildings;

    // Какие здания уже построены
    private Dictionary<BuildingType, bool> built =
        new Dictionary<BuildingType, bool>();

    public event System.Action OnBuildingsChanged;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;

        // Все здания изначально не построены
        foreach (BuildingData b in availableBuildings)
            built[b.buildingType] = false;
    }

    public bool IsBuilt(BuildingType type)
    {
        return built.ContainsKey(type) && built[type];
    }

    public bool Build(BuildingData building)
    {
        if (IsBuilt(building.buildingType))
        {
            Debug.Log($"{building.buildingName} уже построено.");
            return false;
        }

        if (!ResourceManager.Instance.SpendGold(building.cost))
        {
            Debug.Log($"Не хватает золота на {building.buildingName}.");
            return false;
        }

        built[building.buildingType] = true;
        Debug.Log($"Построено: {building.buildingName}");

        OnBuildingsChanged?.Invoke();
        return true;
    }
}