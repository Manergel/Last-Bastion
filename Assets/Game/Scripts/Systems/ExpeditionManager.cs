using UnityEngine;

public class ExpeditionManager : MonoBehaviour
{
    public static ExpeditionManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void SendOnExpedition(UnitInstance unit)
    {
        if (unit == null || unit.isDead) return;

        // Проверяем можно ли идти
        if (!unit.CanGoOnExpedition()) return;

        GameStateManager.Instance.SwitchPhase(GamePhase.Expedition);

        int chance = CalcSuccessChance(unit);
        bool survived = Random.Range(0, 100) < chance;

        if (survived)
        {
            Debug.Log($"{unit.unitName} вернулся. (шанс был {chance}%)");
            unit.SetFatigued(2);
            ResourceManager.Instance.AddGold(20);

            // Снижаем скверну в самом опасном регионе
            RegionData dangerous = GetMostCorruptedRegion();
            if (dangerous != null)
            {
                CorruptionSystem.Instance.ReduceCorruption(dangerous, 1);
                Debug.Log($"Скверна в {dangerous.regionName} снижена.");
            }
        }
        else
        {
            Debug.Log($"{unit.unitName} погиб. (шанс был {chance}%)");

            // Мораль падает у всех выживших
            foreach (UnitInstance survivor in RosterManager.Instance.GetUnits())
            {
                if (survivor != unit)
                    survivor.TakeMoraleDamage(10);
            }

            RosterManager.Instance.RemoveUnit(unit);
            unit.Die();
        }

        GameStateManager.Instance.SwitchPhase(GamePhase.Result);
    }

    public int CalcSuccessChance(UnitInstance unit)
    {
        int chance = 70;

        // Штраф за низкую мораль
        if (unit.currentMorale < 50)
            chance -= 15;

        return Mathf.Clamp(chance, 5, 95);
    }
    
    RegionData GetMostCorruptedRegion()
    {
        RegionData result = null;
        int max = -1;

        foreach (RegionData r in CorruptionSystem.Instance.regions)
        {
            if (!r.isConquered && r.currentCorruption > max)
            {
                max = r.currentCorruption;
                result = r;
            }
        }
        return result;
    }
}