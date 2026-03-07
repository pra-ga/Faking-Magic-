using UnityEngine;
using UnityEngine.Events;

public class QuestSocket : MonoBehaviour
{
    public ItemType acceptedType;
    public bool isFilled = false;
    public UnityEvent OnItemPlaced;
    public WellQuestController controller;
    public string acceptedItemName;

    public bool TryAcceptItem(CarryableItem item)
    {
        //Temporarily disabled
        if (!isFilled && item.type == acceptedType)
        {
            isFilled = true;
            item.isSlotted = true;
            item.transform.SetParent(this.transform);
            item.transform.localPosition = Vector3.zero;
            OnItemPlaced?.Invoke();
            return true;
        }

        if (item.itemName == acceptedItemName)
        {
            // Pass item.itemName dynamically so it's not hardcoded to "Gravel"
            controller.ItemPlaced(item.itemName);
            
            // If you are using UnityEvents, call them like this:
            // onItemPlaced.Invoke(item.itemName); 

            Destroy(item.gameObject);
            return true;
        }
        return false;

    }

    /* public bool TryAcceptItem(CarryableItem item)
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

        if (item.itemName == acceptedItemName)
        {
            controller.ItemPlaced(item.itemName); // Tell the quest controller
            Destroy(item.gameObject); // Make it "vanish" from hand
            return true;
        }

        return false;
    } */
}