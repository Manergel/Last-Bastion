using UnityEngine;

public class HireSystem : MonoBehaviour
{
    // Массив имён — откуда берём случайное имя при найме
    private string[] names = { "Зора", "Карн", "Эйла", "Торвис", "Дэкс" };

    public UnitInstance HireUnit()
    {
        // Создаём пустой объект в сцене — он будет "телом" юнита
        GameObject go = new GameObject();

        // Добавляем компонент UnitInstance на этот объект
        UnitInstance unit = go.AddComponent<UnitInstance>();

        // Даём случайное имя из массива
        unit.unitName = names[Random.Range(0, names.Length)];
        
        RosterManager.Instance.AddUnit(unit);

        Debug.Log($"Нанят: {unit.unitName}");
        return unit;
    }
}