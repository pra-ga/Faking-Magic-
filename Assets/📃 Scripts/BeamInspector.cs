using UnityEngine;

public class BeamInspector : MonoBehaviour
{
    [SerializeField] private MillQuestController questController;

    public void Interact(PlayerInteraction player)
    {
        if (player.currentItem == null)
        {
            questController.OnInspectBeam(); // Show the bubble
        }
    }

    // NEW: Hide the bubble when the player walks away from the beam
    private void OnTriggerExit2D(Collider2D other)
    {
        //Debug.Log((questController != null) + "questController != null");
        if (other.CompareTag("Player") && questController != null)
        {
            //Debug.Log("player walks away from the beam");
            questController.HideVision(); //Hide bubble
        }
    }
}