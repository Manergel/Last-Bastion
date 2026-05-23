using UnityEngine;
using UnityEngine.UI;

public class TestController : MonoBehaviour
{
    private HireSystem hireSystem;
    private ExpeditionManager expeditionManager;
    private UnitInstance currentUnit;

    // Перетащи кнопки из Hierarchy в Inspector
    public Button hireButton;
    public Button sendButton;

    void Start()
    {
        hireSystem = GetComponent<HireSystem>();
        expeditionManager = GetComponent<ExpeditionManager>();
    
        Debug.Log($"HireSystem: {hireSystem}, Expedition: {expeditionManager}");
        

        hireButton.onClick.AddListener(OnHireClicked);
    }

    void OnHireClicked()
    {
        UnitInstance unit = hireSystem.HireUnit();
    }


}