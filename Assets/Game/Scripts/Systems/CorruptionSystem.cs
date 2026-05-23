using UnityEngine;

public class CorruptionSystem : MonoBehaviour
{
    public static CorruptionSystem Instance { get; private set; }

    public RegionData[] regions; // перетащи три региона в Inspector

    public event System.Action OnCorruptionChanged;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;

        // Сбрасываем все регионы при старте игры
        foreach (RegionData region in regions)
            region.Reset();
    }

    // Вызывается из TurnManager каждый ход
    public void Tick()
    {
        // Каждые 3 хода скверна растёт в случайном незахваченном регионе
        if (TurnManager.Instance.CurrentTurn % 3 != 0) return;

        RegionData target = GetRandomActiveRegion();
        if (target == null) return;

        target.AddCorruption(1);
        Debug.Log($"Скверна растёт в {target.regionName}. Уровень: {target.currentCorruption}/3");

        OnCorruptionChanged?.Invoke();
    }

    public void ReduceCorruption(RegionData region, int amount)
    {
        region.ReduceCorruption(amount);
        OnCorruptionChanged?.Invoke();
    }

    public bool IsGameOver()
    {
        foreach (RegionData region in regions)
            if (!region.isConquered) return false;

        return true; // все регионы захвачены
    }

    RegionData GetRandomActiveRegion()
    {
        // Собираем незахваченные регионы
        System.Collections.Generic.List<RegionData> active =
            new System.Collections.Generic.List<RegionData>();

        foreach (RegionData r in regions)
            if (!r.isConquered) active.Add(r);

        if (active.Count == 0) return null;
        return active[Random.Range(0, active.Count)];
    }
}