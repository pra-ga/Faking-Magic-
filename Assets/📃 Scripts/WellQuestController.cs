using UnityEngine;

public class WellQuestController : MonoBehaviour
{
    [Header("Quest States")]
    //public GameObject installedReeds;
    //public GameObject bucketFilledVisual;
    //public GameObject scrapMetalReward;

    [Header("Quest Visuals")]
    public GameObject hollowReedsVisual; // GO inside the bucket
    public GameObject waterFill;         // The "Success" water GO
    
    [Header("Dialogue")]
    public DialogueUIController dialogueUI;
    public DialogueData miracleDialogue; // "Drink your fill magician!"
    
    private bool hasSand = false;
    private bool hasGravel = false;
    private bool hasReeds = false;
    public bool isCompleted = false;

    public void ItemPlaced(string itemName)
    {
        Debug.Log("ItemPlaced"+ itemName);
        if (itemName == "Gravel") {
            Debug.Log("HAs gravel");
            hasGravel = true;
        }
        else if (itemName == "Reed") 
        {
            Debug.Log("HAs reed");
            hasReeds = true;
            hollowReedsVisual.SetActive(true); // Step 9
        }

        CheckCompletion();
    }

    private void CheckCompletion()
    {
        if (hasGravel && hasReeds && !isCompleted)
        {
            isCompleted = true;
            waterFill.SetActive(true); // Step 10
            dialogueUI.ShowDialogue(miracleDialogue);
        }
    }

    /* private void CompleteQuest()
    {
        isCompleted = true;
        
        if (bucketFilledVisual != null) bucketFilledVisual.SetActive(true);
        if (scrapMetalReward != null) scrapMetalReward.SetActive(true);

        // Auto-trigger the "Miracle" dialogue
        if (dialogueUI != null) dialogueUI.ShowDialogue(miracleDialogue);
        
        Debug.Log("Quest 2 Complete: Filter and Siphon active!");
    } */
}