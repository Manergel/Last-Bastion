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
        // Если юнит уже мёртв — игнорируем
        if (unit == null || unit.isDead) return;

        GameStateManager.Instance.SwitchPhase(GamePhase.Expedition);

        // Бросаем монетку: больше 0.5 — выжил
        bool survived = Random.value > 0.5f;

        if (survived)
        {
            Debug.Log($"{unit.unitName} вернулся.");
        }
        else
        {
            RosterManager.Instance.RemoveUnit(unit);
            unit.Die();
        }
        
        GameStateManager.Instance.SwitchPhase(GamePhase.Result);
    }
}