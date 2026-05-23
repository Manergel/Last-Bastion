using UnityEngine;

public enum BuildingType
{
    Tavern,       // Таверна — больше выбор при найме
    Infirmary,    // Лазарет — усталость проходит быстрее
    Watchtower    // Башня — скверна растёт медленнее
}

[CreateAssetMenu(fileName = "Building", menuName = "LastBastion/Building")]
public class BuildingData : ScriptableObject
{
    public string buildingName;
    public string description;
    public BuildingType buildingType;
    public int cost;
}