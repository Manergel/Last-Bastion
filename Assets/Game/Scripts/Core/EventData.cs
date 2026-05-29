using UnityEngine;

[CreateAssetMenu(fileName = "Event", menuName = "LastBastion/Event")]
public class EventData : ScriptableObject
{
    public string eventTitle;
    
    [TextArea(3, 5)]
    public string eventDescription;
    
    public ChoiceData[] choices; // два варианта действий
}

[System.Serializable]
public class ChoiceData
{
    public string choiceText;        // "Атаковать в лоб"
    public string successText;       // "Отряд прорвался"
    public string failureText;       // "Засада — потери"
    public int baseSuccessChance;    // базовый шанс 0-100
    public int goldReward;           // награда за успех
    public bool canCauseDeath;       // может ли убить юнита
}