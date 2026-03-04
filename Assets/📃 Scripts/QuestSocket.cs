using UnityEngine;
using UnityEngine.Events;

public class QuestSocket : MonoBehaviour
{
    public ItemType acceptedType;
    public bool isFilled = false;
    public UnityEvent OnItemPlaced;

    public bool TryAcceptItem(CarryableItem item)
    {
        if (!isFilled && item.type == acceptedType)
        {
            isFilled = true;
            item.isSlotted = true;
            item.transform.SetParent(this.transform);
            item.transform.localPosition = Vector3.zero;
            OnItemPlaced?.Invoke();
            return true;
        }
        return false;
    }
}