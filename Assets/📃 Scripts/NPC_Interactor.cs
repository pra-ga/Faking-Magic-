using UnityEngine;

public class NPC_Interactor : MonoBehaviour 
{
    [SerializeField] private MillQuestController questController;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the entering object is the Player
        /* if (other.CompareTag("Player") && questController != null)
        {
            questController.OnTalkToBaker(); // Show the bubble
        } */
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // When the player walks away, hide the dialogue and the vision
        if (other.CompareTag("Player") && questController != null)
        {
            // Force close the dialogue UI if it's active
            if (questController.dialogueUI != null)
                questController.dialogueUI.gameObject.SetActive(false);

            questController.HideVision(); 
        }
    }

    public void Interact()
    {
        // Dialogue only triggers when the player intentionally interacts
        if (questController != null) 
            questController.OnTalkToBaker();
    }
}