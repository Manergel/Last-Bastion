using UnityEngine;

public class BuildingsUI : MonoBehaviour
{
    public BuildingButtonUI[] buildingButtons;
    public BuildingData[] buildings;

    void Start()
    {
        for (int i = 0; i < buildingButtons.Length; i++)
            buildingButtons[i].Setup(buildings[i]);
    }
}