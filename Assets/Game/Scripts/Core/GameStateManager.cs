using System;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    public GamePhase CurrentPhase { get; private set; }

    public event Action<GamePhase> OnPhaseChanged;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        SwitchPhase(GamePhase.Base);
    }

    public void SwitchPhase(GamePhase newPhase)
    {
        if (CurrentPhase == newPhase)
            return;

        ExitPhase(CurrentPhase);

        CurrentPhase = newPhase;

        EnterPhase(CurrentPhase);

        Debug.Log("Phase switched to: " + CurrentPhase);

        OnPhaseChanged?.Invoke(CurrentPhase);
    }

    private void EnterPhase(GamePhase phase)
    {
        switch (phase)
        {
            case GamePhase.Base:
                Debug.Log("Entering Base");
                break;

            case GamePhase.Expedition:
                Debug.Log("Entering Expedition");
                break;

            case GamePhase.Combat:
                Debug.Log("Entering Combat");
                break;
        }
    }

    private void ExitPhase(GamePhase phase)
    {
        switch (phase)
        {
            case GamePhase.Base:
                Debug.Log("Exiting Base");
                break;

            case GamePhase.Expedition:
                Debug.Log("Exiting Expedition");
                break;

            case GamePhase.Combat:
                Debug.Log("Exiting Combat");
                break;
        }
    }
}