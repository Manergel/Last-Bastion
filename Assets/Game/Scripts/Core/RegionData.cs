using UnityEngine;

[CreateAssetMenu(fileName = "Region", menuName = "LastBastion/Region")]
public class RegionData : ScriptableObject
{
    public string regionName;
    public int currentCorruption = 0; // 0-3
    public bool isConquered = false;

    public void AddCorruption(int amount)
    {
        if (isConquered) return;
        currentCorruption = Mathf.Clamp(currentCorruption + amount, 0, 3);
        if (currentCorruption >= 3)
        {
            isConquered = true;
            Debug.Log($"{regionName} захвачен скверной.");
        }
    }

    public void ReduceCorruption(int amount)
    {
        if (isConquered) return;
        currentCorruption = Mathf.Max(0, currentCorruption - amount);
    }

    // Сброс для новой игры
    public void Reset()
    {
        currentCorruption = 0;
        isConquered = false;
    }
}