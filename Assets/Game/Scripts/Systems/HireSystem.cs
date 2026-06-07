using UnityEngine;

public class HireSystem : MonoBehaviour
{
    private const int HIRE_COST = 15;
    
    // Массив имён — откуда берём случайное имя при найме
    private string[] baseNames = { "Зора", "Карн", "Эйла", "Торвис", "Дэкс" };
    private string[] extraNames = { "Рейна", "Вольт", "Сера", "Брик", "Нэйл", "Астра" };

    public UnitInstance HireUnit()
    {
        if (!ResourceManager.Instance.SpendGold(HIRE_COST))
        {
            Debug.Log("Не хватает золота!");
            return null;
        }

        string[] pool = BuildingSystem.Instance != null &&
                        BuildingSystem.Instance.IsBuilt(BuildingType.Tavern)
            ? extraNames : baseNames;

        GameObject go = new GameObject();
        UnitInstance unit = go.AddComponent<UnitInstance>();
        unit.unitName = pool[Random.Range(0, pool.Length)];
    
        // Случайный класс при найме
        UnitClass[] classes = (UnitClass[])System.Enum.GetValues(typeof(UnitClass));
        unit.unitClass = classes[Random.Range(0, classes.Length)];
    
        Debug.Log($"Нанят: {unit.unitName} [{unit.ClassName}] за {HIRE_COST} золота.");

        RosterManager.Instance.AddUnit(unit);
        return unit;
    }
}