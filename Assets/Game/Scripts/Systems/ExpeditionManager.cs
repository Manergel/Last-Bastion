using System.Collections.Generic;
using UnityEngine;

public class ExpeditionManager : MonoBehaviour
{
    public static ExpeditionManager Instance { get; private set; }

    [Header("События для экспедиций")]
    public EventData[] availableEvents; // перетащи сюда созданные события

    public List<UnitInstance> currentSquad = new List<UnitInstance>();
    
    private List<ExpeditionNode> nodes = new List<ExpeditionNode>();
    private int currentNodeIndex = 0;
    
    public int CurrentNodeIndex => currentNodeIndex;
    public int TotalNodes => nodes.Count;
    public bool IsLastNode => currentNodeIndex >= nodes.Count - 1;

    // Событие — UI подпишется
    public event System.Action<NodeResult> OnNodeResolved;
    public event System.Action OnExpeditionEnded;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void StartExpedition(List<UnitInstance> squad)
    {
        currentSquad = new List<UnitInstance>(squad);
        currentNodeIndex = 0;
        nodes.Clear();

        // Создаём 3 узла из случайных событий
        for (int i = 0; i < 3; i++)
        {
            EventData randomEvent = availableEvents[Random.Range(0, availableEvents.Length)];
            ExpeditionNode node = new ExpeditionNode();
            node.eventData = randomEvent;
            node.isLastNode = (i == 2);
            nodes.Add(node);
        }

        Debug.Log($"Экспедиция началась. Узлов: {nodes.Count}");
    }

    public ExpeditionNode GetCurrentNode()
    {
        if (currentNodeIndex >= nodes.Count) return null;
        return nodes[currentNodeIndex];
    }

    public NodeResult ResolveCurrentNode(int choiceIndex)
    {
        ExpeditionNode node = GetCurrentNode();
        if (node == null) return null;

        NodeResult result = node.ResolveChoice(choiceIndex, currentSquad);

        // Применяем последствия
        if (result.goldEarned > 0)
            ResourceManager.Instance.AddGold(result.goldEarned);

        if (result.deadUnit != null)
        {
            currentSquad.Remove(result.deadUnit);
            RosterManager.Instance.RemoveUnit(result.deadUnit);
            
            // Мораль падает у всех
            foreach (UnitInstance unit in currentSquad)
                unit.TakeMoraleDamage(10);
                
            result.deadUnit.Die();
        }

        OnNodeResolved?.Invoke(result);
        return result;
    }

    public void AdvanceToNextNode()
    {
        currentNodeIndex++;
    }

    public void EndExpedition()
    {
        // Усталость всем выжившим
        foreach (UnitInstance unit in currentSquad)
            unit.SetFatigued(2);

        OnExpeditionEnded?.Invoke();
        Debug.Log("Экспедиция завершена.");
    }

    public int CalcSuccessChance(UnitInstance unit)
    {
        int chance = 70;
        if (unit.currentMorale < 50) chance -= 15;
        return Mathf.Clamp(chance, 5, 95);
    }

    public void SendOnExpedition(UnitInstance unit)
    {
        if (unit == null || unit.isDead) return;
        if (!unit.CanGoOnExpedition()) return;
    }

    RegionData GetMostCorruptedRegion()
    {
        if (CorruptionSystem.Instance == null) return null;

        RegionData result = null;
        int max = -1;

        foreach (RegionData r in CorruptionSystem.Instance.regions)
        {
            if (!r.isConquered && r.currentCorruption > max)
            {
                max = r.currentCorruption;
                result = r;
            }
        }
        return result;
    }
}