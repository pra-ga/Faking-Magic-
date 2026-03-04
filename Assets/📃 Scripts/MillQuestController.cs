using System.Collections;
using UnityEngine;

/* State: Locked/Idle – Player hasn't talked to the Baker. The Mill gears are jammed.

State: Active – Player talked to the Baker. The Thought Bubble (Vision) is active.

State: PulleyInstalled – The player has brought the Pulley from the well and slotted it into the "Hook Socket."

State: RopeInstalled – The player has brought the Rope from the barn and slotted it into the "Beam Socket."

State: Ready – Both items are placed. The player interacts with the rope to "Pull."

State: Completed – Animation plays (beam slides out), Baker drops Flour, Vision disappears. */

public class MillQuestController : MonoBehaviour
{
    public GameObject thoughtBubble;
    public GameObject pulleyVisualOnBubble;
    public GameObject ropeVisualOnBubble;
    
    [Header("Quest Objects")]
    public GameObject ironBeam;
    public GameObject flourPrefab;
    public Transform flourSpawnPoint;

    private bool hasPulley = false;
    private bool hasRope = false;
    private bool isCompleted = false;

    public void OnTalkToBaker()
    {
        if (!isCompleted) {
            thoughtBubble.SetActive(true);
            Debug.Log("OnTalkToBaker()");
        }
    }

    public void PulleySlotted()
    {
        hasPulley = true;
        pulleyVisualOnBubble.SetActive(false); // Check off in vision
        Debug.Log("PulleySlotted()");
        CheckCompletion();
    }

    public void RopeSlotted()
    {
        hasRope = true;
        ropeVisualOnBubble.SetActive(false); // Check off in vision
        Debug.Log("RopeSlotted()");
        CheckCompletion();
    }

    private void CheckCompletion()
    {
        if (hasPulley && hasRope)
        {
            CompleteQuest();
        }
    }

    private void CompleteQuest()
    {
        isCompleted = true;
        thoughtBubble.SetActive(false);
        Debug.Log("CompleteQuest()");
        
        // Simple "Beam Slide" animation logic
        // Start the manual movement process
        StartCoroutine(MoveBeamSequence());;
    }

    private IEnumerator MoveBeamSequence()
    {
        Vector3 startPos = ironBeam.transform.position;
        Vector3 endPos = startPos + new Vector3(3f, 0, 0); // Move 3 units on X
        float duration = 2.0f;
        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float percent = elapsed / duration;

            // Use SmoothStep for a more mechanical "heavy" feel
            float curve = Mathf.SmoothStep(0, 1, percent);
            
            ironBeam.transform.position = Vector3.Lerp(startPos, endPos, curve);
            yield return null; // Wait for next frame
        }

        // Ensure it snaps exactly to the end position
        ironBeam.transform.position = endPos;

        // The "OnComplete" logic
        Instantiate(flourPrefab, flourSpawnPoint.position, Quaternion.identity);
        Debug.Log("Quest 1 Complete: Baker is happy!");
    }

    public void HideVision()
    {
        // Turn off the bubble only if the quest isn't already finished
        if (!isCompleted) 
        {
            thoughtBubble.SetActive(false);
        }
    }
}