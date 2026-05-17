using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    public GamePhase CurrentPhase;
    
    public event Action<GamePhase> OnPhaseChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        SwitchPhase(GamePhase.Base);
    }
    
    private void Update()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            SwitchPhase(GamePhase.Base);
        }

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            SwitchPhase(GamePhase.Preparation);
        }

        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            SwitchPhase(GamePhase.Expedition);
        }

        if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            SwitchPhase(GamePhase.Combat);
        }

        if (Keyboard.current.digit5Key.wasPressedThisFrame)
        {
            SwitchPhase(GamePhase.Result);
        }
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