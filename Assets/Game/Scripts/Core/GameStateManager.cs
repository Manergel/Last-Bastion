using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    // Singleton: одна точка доступа на всю игру
    public static GameStateManager Instance { get; private set; }

    public GamePhase CurrentPhase { get; private set; }

    void Awake()
    {
        // Если менеджер уже есть — уничтожаем дубликат
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void SwitchPhase(GamePhase newPhase)
    {
        CurrentPhase = newPhase;
        Debug.Log($"Фаза: {newPhase}");
    }
}