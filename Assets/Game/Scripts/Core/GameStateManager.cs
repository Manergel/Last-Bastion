using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    // Singleton: одна точка доступа на всю игру
    public static GameStateManager Instance { get; private set; }

    public GamePhase CurrentPhase { get; private set; }

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        CurrentPhase = GamePhase.Base;
    }

    public void SwitchPhase(GamePhase newPhase)
    {
        CurrentPhase = newPhase;
        Debug.Log($"Фаза: {newPhase}");
    }
}