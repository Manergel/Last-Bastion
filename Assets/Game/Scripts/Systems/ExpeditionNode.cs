using UnityEngine;

public class ExpeditionNode
{
    public EventData eventData;
    public bool isCompleted = false;
    public bool isLastNode = false;

    public NodeResult ResolveChoice(int choiceIndex, System.Collections.Generic.List<UnitInstance> squad)
    {
        ChoiceData choice = eventData.choices[choiceIndex];
    
        int chance = choice.baseSuccessChance;
    
        // Мораль отряда
        int avgMorale = 0;
        foreach (UnitInstance unit in squad)
            avgMorale += unit.currentMorale;
        avgMorale /= squad.Count;
    
        if (avgMorale < 50) chance -= 15;
        if (avgMorale > 80) chance += 10;

        // Бонусы классов
        foreach (UnitInstance unit in squad)
        {
            switch (unit.unitClass)
            {
                case UnitClass.Knight:
                    chance += 25; // всегда даёт бонус в бою
                    break;
                case UnitClass.Hunter:
                    if (squad.Count <= 2) chance += 30; // бонус в малом отряде
                    break;
            }
        }
    
        chance = Mathf.Clamp(chance, 5, 95);
    
        bool success = UnityEngine.Random.Range(0, 100) < chance;
    
        NodeResult result = new NodeResult();
        result.success = success;
        result.chanceUsed = chance;
        result.resultText = success ? choice.successText : choice.failureText;
        result.goldEarned = success ? choice.goldReward : 0;
    
        if (!success && choice.canCauseDeath && squad.Count > 0)
        {
            if (UnityEngine.Random.value > 0.6f)
            {
                UnitInstance victim = squad[UnityEngine.Random.Range(0, squad.Count)];
                result.deadUnit = victim;
            }
        }
    
        isCompleted = true;
        return result;
    }
}

// Результат узла — передаём в UI
public class NodeResult
{
    public bool success;
    public int chanceUsed;
    public string resultText;
    public int goldEarned;
    public UnitInstance deadUnit; // null если никто не погиб
}