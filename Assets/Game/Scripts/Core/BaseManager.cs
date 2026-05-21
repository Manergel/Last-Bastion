using UnityEngine;
using UnityEngine.InputSystem;

public class BaseManager : MonoBehaviour
{
    private void Start()
    {
        GameStateManager.Instance.OnPhaseChanged += OnPhaseChanged;
    }

    private void OnDestroy()
    {
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.OnPhaseChanged -= OnPhaseChanged;
    }

    private void OnPhaseChanged(GamePhase phase)
    {
        if (phase == GamePhase.Base)
            EnterBase();
        else
            ExitBase();
    }

    private void EnterBase()
    {
        Debug.Log("🏰 ENTER BASE MODE");
    }

    private void ExitBase()
    {
        Debug.Log("🏰 EXIT BASE MODE");
    }
    

    public void StartExpeditionButton()
    {
        Debug.Log("Starting Expedition...");
        GameStateManager.Instance.SwitchPhase(GamePhase.Expedition);
    }
}