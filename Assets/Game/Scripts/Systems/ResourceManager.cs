using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    public int Gold { get; private set; } = 60; // стартовое золото

    // Событие — UI подпишется и обновится автоматически
    public event System.Action OnGoldChanged;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void AddGold(int amount)
    {
        Gold += amount;
        OnGoldChanged?.Invoke();
    }

    public bool SpendGold(int amount)
    {
        // Если не хватает — возвращаем false, не тратим
        if (Gold < amount) return false;

        Gold -= amount;
        OnGoldChanged?.Invoke();
        return true;
    }

    public bool CanAfford(int amount)
    {
        return Gold >= amount;
    }
}