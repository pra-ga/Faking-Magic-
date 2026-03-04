using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Transform handPivot; // A child empty GO where the item will sit
    public CarryableItem currentItem;
    public float interactRange = 1.5f;
    public LayerMask interactLayer;

    // Called by Player Input "Interact" Action
    public void OnInteract()
    {
        if (currentItem != null)
        {
            TryPlaceItem();
        }
        else
        {
            TryPickUpItem();
        }
    }

    private void TryPickUpItem()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, interactRange, interactLayer);
        
        if (hit)
        {
            // Check if we hit an NPC first
            if (hit.TryGetComponent(out NPC_Interactor npc))
            {
                npc.Interact();
                return; // Stop here so we don't try to "pick up" the baker!
            }

            // Check for items
            if (hit.TryGetComponent(out CarryableItem item) && !item.isSlotted)
            {
                currentItem = item;
                item.OnPickedUp();
                item.transform.SetParent(handPivot);
                item.transform.localPosition = Vector3.zero;
            }
        }
    }

    private void TryPlaceItem()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, interactRange, interactLayer);
        if (hit && hit.TryGetComponent(out QuestSocket socket))
        {
            if (socket.TryAcceptItem(currentItem))
            {
                currentItem = null; // Item is now owned by the socket
            }
        }
        else
        {
            // Drop on ground if no socket found
            currentItem.transform.SetParent(null);
            currentItem.OnDropped();
            currentItem = null;
        }
    }
}