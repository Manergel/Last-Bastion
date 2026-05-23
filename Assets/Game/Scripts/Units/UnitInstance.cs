using UnityEngine;

public class UnitInstance : MonoBehaviour
{
    public string unitName;
    public bool isDead = false;

    // Мораль 0-100, усталость в ходах
    public int currentMorale = 100;
    public bool isFatigued = false;
    public int fatigueTurnsLeft = 0;

    public event System.Action OnStatsChanged; // UI подпишется и обновится

    public void Die()
    {
        isDead = true;
        Debug.Log($"{unitName} погиб навсегда.");
        Destroy(gameObject);
    }

    public void TakeMoraleDamage(int amount)
    {
        currentMorale = Mathf.Clamp(currentMorale - amount, 0, 100);
        OnStatsChanged?.Invoke();
    }

    public void SetFatigued(int turns)
    {
        isFatigued = true;
        fatigueTurnsLeft = turns;
        OnStatsChanged?.Invoke();
    }

    // Вызывается каждый ход из FatigueSystem
    public void TickFatigue()
    {
        if (!isFatigued) return;

        fatigueTurnsLeft--;

        if (fatigueTurnsLeft <= 0)
        {
            isFatigued = false;
            fatigueTurnsLeft = 0;
            Debug.Log($"{unitName} отдохнул.");
        }

        OnStatsChanged?.Invoke();
    }

    public bool CanGoOnExpedition()
    {
        // Нельзя отправить если устал или сломлен морально
        if (isFatigued)
        {
            Debug.Log($"{unitName} устал — нужен отдых.");
            return false;
        }
        if (currentMorale <= 0)
        {
            Debug.Log($"{unitName} сломлен — мораль на нуле.");
            return false;
        }
        return true;
    }
}