using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }

    public int CurrentTurn { get; private set; } = 1;

    public event System.Action OnTurnEnded;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void EndTurn()
    {
        CurrentTurn++;
        Debug.Log($"Ход {CurrentTurn}");

        foreach (UnitInstance unit in RosterManager.Instance.GetUnits())
        {
            unit.TickFatigue();
            unit.TakeMoraleDamage(-5); // отрицательный урон = восстановление
        }

        OnTurnEnded?.Invoke();
    }
}