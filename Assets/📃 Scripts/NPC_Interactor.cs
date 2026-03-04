using UnityEngine;

public class NPC_Interactor : MonoBehaviour 
{
    [SerializeField] private MillQuestController questController;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the entering object is the Player
        if (other.CompareTag("Player") && questController != null)
        {
            questController.OnTalkToBaker(); // Show the bubble
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Turn off the bubble when the player walks away
        if (other.CompareTag("Player") && questController != null)
        {
            questController.HideVision(); 
        }
    }

    public void Interact()
    {
        // Keep this for actual dialogue or specific logic if needed
        if (questController != null) questController.OnTalkToBaker();
    }
}