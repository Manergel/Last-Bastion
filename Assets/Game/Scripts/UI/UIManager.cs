using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Панели")]
    public GameObject unitSelectionPanel;
    public GameObject expeditionPanel;
    public GameObject extractionPanel;
    public GameObject resultPanel;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        ShowPanel(unitSelectionPanel); // начинаем с выбора отряда
    }

    public void ShowPanel(GameObject panel)
    {
        // Выключаем все панели
        unitSelectionPanel.SetActive(false);
        expeditionPanel.SetActive(false);
        extractionPanel.SetActive(false);
        resultPanel.SetActive(false);

        // Включаем нужную
        panel.SetActive(true);
    }
}