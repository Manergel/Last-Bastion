using UnityEngine;

public class ExpeditionManager : MonoBehaviour
{
    public static ExpeditionManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    // Считаем шанс — сюда позже придут бонусы снаряжения и штрафы усталости
    public int CalcSuccessChance(UnitInstance unit)
    {
        int chance = 50; // базовый шанс
        return Mathf.Clamp(chance, 5, 95); // минимум 5%, максимум 95%
    }

    public void SendOnExpedition(UnitInstance unit)
    {
        if (unit == null || unit.isDead) return;

        GameStateManager.Instance.SwitchPhase(GamePhase.Expedition);

        int chance = CalcSuccessChance(unit);
        bool survived = Random.Range(0, 100) < chance;

        if (survived)
        {
            Debug.Log($"{unit.unitName} вернулся. (шанс был {chance}%)");
        }
        else
        {
            Debug.Log($"{unit.unitName} погиб. (шанс был {chance}%)");
            RosterManager.Instance.RemoveUnit(unit);
            unit.Die();
        }

        GameStateManager.Instance.SwitchPhase(GamePhase.Result);
    }
}