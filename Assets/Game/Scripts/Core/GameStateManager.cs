using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }
    public GamePhase CurrentPhase { get; private set; }

    [Header("Стартовые юниты")]
    public int startingUnitCount = 4;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        CurrentPhase = GamePhase.Base;
    }

    void Start()
    {
        HireSystem hireSystem = GetComponent<HireSystem>();

        if (hireSystem == null)
        {
            Debug.LogError("HireSystem не найден на GameManager!");
            return;
        }

        for (int i = 0; i < startingUnitCount; i++)
            hireSystem.HireUnit();
    }

    public void SwitchPhase(GamePhase newPhase)
    {
        CurrentPhase = newPhase;
        Debug.Log($"Фаза: {newPhase}");
    }
}