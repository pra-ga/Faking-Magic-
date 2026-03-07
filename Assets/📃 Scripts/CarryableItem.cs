using UnityEngine;

public class CarryableItem : MonoBehaviour
{
    public string itemName;
    public ItemType type;
    public bool isSlotted = false;

    public void OnPickedUp()
    {
        GetComponent<Collider2D>().enabled = false; // Prevent bumping into player
    }

    public void OnDropped()
    {
        GetComponent<Collider2D>().enabled = true;
    }
}