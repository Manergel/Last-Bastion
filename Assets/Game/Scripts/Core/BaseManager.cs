using UnityEngine;

public class BaseManager : MonoBehaviour
{
    private void Start()
    {
        // безопасно подписываемся, когда GameStateManager уже точно существует
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnPhaseChanged += OnPhaseChanged;
        }
        else
        {
            Debug.LogError("GameStateManager is NULL in BaseManager");
        }
    }

    private void OnDestroy()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnPhaseChanged -= OnPhaseChanged;
        }
    }

    private void OnPhaseChanged(GamePhase phase)
    {
        if (phase == GamePhase.Base)
        {
            EnterBase();
        }
        else
        {
            ExitBase();
        }
    }

    private void EnterBase()
    {
        Debug.Log("🏰 ENTER BASE MODE");
    }

    private void ExitBase()
    {
        Debug.Log("🏰 EXIT BASE MODE");
    }

    // ТЕСТОВАЯ КНОПКА (пока без UI)
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartExpeditionButton();
        }
    }

    public void StartExpeditionButton()
    {
        Debug.Log("Starting Expedition...");
        GameStateManager.Instance.SwitchPhase(GamePhase.Expedition);
    }
}