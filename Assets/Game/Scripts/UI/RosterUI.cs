using UnityEngine;

public class RosterUI : MonoBehaviour
{
    public Transform container;       // куда складываем карточки
    public UnitCardUI cardPrefab;     // шаблон карточки

    void Start()
    {
        // Подписываемся на событие — когда список меняется, перерисовываем
        RosterManager.Instance.OnRosterChanged += RefreshUI;
    }

    void OnDestroy()
    {
        // Отписываемся когда объект уничтожается — иначе утечка памяти
        RosterManager.Instance.OnRosterChanged -= RefreshUI;
    }

    void RefreshUI()
    {
        // Удаляем все старые карточки
        foreach (Transform child in container)
            Destroy(child.gameObject);

        // Создаём новые по текущему списку
        foreach (UnitInstance unit in RosterManager.Instance.GetUnits())
        {
            UnitCardUI card = Instantiate(cardPrefab, container);
            card.Setup(unit);
        }
    }
}