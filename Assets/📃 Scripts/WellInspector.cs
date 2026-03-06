using UnityEngine;

public class WellInspector : MonoBehaviour
{
    [SerializeField] private WellInteraction wellLogic;

    public void Interact(PlayerInteraction player)
    {
        // If player is empty handed, show them what they need
        if (player.currentItem == null)
        {
            wellLogic.ShowWellVision();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            wellLogic.HideWellVision();
        }
    }
}