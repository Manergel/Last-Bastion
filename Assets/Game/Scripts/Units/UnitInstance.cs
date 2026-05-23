using UnityEngine;

public class UnitInstance : MonoBehaviour
{
    public string unitName;
    public bool isDead = false;

    public void Die()
    {
        isDead = true;
        Debug.Log($"{unitName} погиб навсегда.");
        Destroy(gameObject);
    }
}