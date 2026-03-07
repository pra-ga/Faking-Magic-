using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Transform handPivot; // A child empty GO where the item will sit
    public CarryableItem currentItem;
    public float interactRange = 1.5f;
    public LayerMask interactLayer;

    [Header("Dialogue Integration")]
    public DialogueUIController dialogueUI;

    // Called by Player Input "Interact" Action
    public void OnInteract()
    {
        if (dialogueUI != null && dialogueUI.gameObject.activeInHierarchy)
        {
            dialogueUI.DisplayNextLine();
            return; 
        }

        // 1. Try to Harvest/Dig first (Matches your Sand/Spade logic)
        Collider2D hit = Physics2D.OverlapCircle(transform.position, interactRange, interactLayer);
        if (hit && hit.TryGetComponent(out ResourceSource source))
        {
            source.OnHarvest(this);
            return; 
        }

        // 2. If not digging, check if we should place or pick up
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

            // 1. Resource Harvesting (Sand, Reeds, etc)
            if (hit.TryGetComponent(out ResourceSource source))
            {
                source.OnHarvest(this);
                return;
            }

            // 2. Bucket Vision (If empty handed, show siphon thought)
            if (hit.CompareTag("Bucket") && currentItem == null)
            {
                // Assuming the WellInteraction script handles the specific vision toggle
                hit.GetComponent<WellInteraction>().ShowWellVision(); 
                return;
            }

            // 2. Check for the Well Inspector
            if (hit.TryGetComponent(out WellInspector well)) { well.Interact(this); return; }

            // 2. Check for the Beam
            if (hit.TryGetComponent(out BeamInspector beam))
            {
                beam.Interact(this); // Pass 'this' so it can check currentItem
                return;
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

    public void OnDrop()
    {
        // Only drop if we are holding something and not in the middle of a dialogue
        if (currentItem != null && (dialogueUI == null || !dialogueUI.gameObject.activeInHierarchy))
        {
            DropItem();
        }
    }

    private void TryPlaceItem()
    {
        // Use OverlapCircleAll to find EVERYTHING in range
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactRange, interactLayer);
        
        foreach (var hit in hits)
        {
            // Skip the item currently in our hand
            if (currentItem != null && hit.gameObject == currentItem.gameObject) continue;

            if (hit.TryGetComponent(out QuestSocket socket))
            {
                if (socket.TryAcceptItem(currentItem))
                {
                    currentItem = null; 
                    return; // Exit once successfully placed
                }
            }
        }
    }

    /* private void TryPlaceItem()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, interactRange, interactLayer);
        
        // If we hit a socket, try to place it
        if (hit && hit.TryGetComponent(out QuestSocket socket))
        {
            if (socket.TryAcceptItem(currentItem))
            {
                currentItem = null; 
            }
        }
        // NOTE: We no longer drop the item automatically here if the socket check fails.
    } */

    private void DropItem()
    {
        if (currentItem == null) return;

        currentItem.transform.SetParent(null);
        currentItem.OnDropped();
        currentItem = null;
        Debug.Log("Item dropped manually.");
    }
}