using System.Collections.Generic;
using UnityEngine;

public class RosterManager : MonoBehaviour
{
    public static RosterManager Instance { get; private set; }

    // Список всех живых юнитов
    private List<UnitInstance> units = new List<UnitInstance>();

    // Событие — кто-то подпишется и узнает когда список изменился
    public event System.Action OnRosterChanged;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void AddUnit(UnitInstance unit)
    {
        units.Add(unit);
        OnRosterChanged?.Invoke(); // оповещаем всех подписчиков
    }

    public void RemoveUnit(UnitInstance unit)
    {
        units.Remove(unit);
        OnRosterChanged?.Invoke();
    }

    // Возвращаем копию списка — чтобы никто не менял его напрямую
    public List<UnitInstance> GetUnits()
    {
        return new List<UnitInstance>(units);
    }
}