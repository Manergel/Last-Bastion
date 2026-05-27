using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    // Singleton: одна точка доступа на всю игру
    public static GameStateManager Instance { get; private set; }

    public GamePhase CurrentPhase { get; private set; }
    [Header("Стартовые юниты")]
    public int startingUnitCount = 4;

    void Start()
    {
        // Создаём стартовый отряд
        for (int i = 0; i < startingUnitCount; i++)
            FindObjectOfType<HireSystem>().HireUnit();
    }
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