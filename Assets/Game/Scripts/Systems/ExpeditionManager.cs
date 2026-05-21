using UnityEngine;

public class ExpeditionManager : MonoBehaviour
{
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
            GameStateManager.Instance.SwitchPhase(GamePhase.Result);
        }
        else
        {
            unit.Die();
            GameStateManager.Instance.SwitchPhase(GamePhase.Result);
        }
    }
}