using System.Collections;
using UnityEngine;

public class MillQuestController : MonoBehaviour
{
    /* public GameObject thoughtBubble;
    public GameObject pulleyVisualOnBubble;
    public GameObject ropeVisualOnBubble; */

    [Header("New Vision System")]
    public GameObject solutionThoughtBubble;
    
    [Header("Quest Objects")]
    public GameObject ironBeam;
    public GameObject flourPrefab;
    public Transform flourSpawnPoint;

    [Header("Visual Feedback")]
    public GameObject strungRopeVisual; // The rope looping through the pulley
    private GameObject physicalRopeInSocket; // The temporary rope found in the barn

    [Header("Movement Sequence Objects")]
    public Transform bakerTransform;
    public Animator bakerAnimator; // Reference to Baker's Animator for "isPulling"
    public Transform hookTransform;
    [SerializeField] GameObject bakerSadFace;
    [SerializeField] GameObject bakerHappyFace;

    [Header("Dialogue System")]
    public DialogueUIController dialogueUI;
    public DialogueData introDialogue;    // "The bandits jammed the gears!"
    //public DialogueData completeDialogue; // "A miracle! The beam moves!"

    [Header("Illusion Theme Dialogue")]
    public DialogueData miracleDialogue; // "A miracle! He moved the iron with a word!"
    public DialogueData permanentMagicianDialogue; // "Ah, the Great Magician returns!"

    private bool hasPulley = false;
    private bool hasRope = false;
    private bool isCompleted = false;

    void Start()
    {
        bakerSadFace.SetActive(true);
        bakerHappyFace.SetActive(false);
    }

    public void OnTalkToBaker()
    {
        if (dialogueUI == null) return;

        if (!isCompleted) 
        dialogueUI.ShowDialogue(introDialogue);
        else 
        dialogueUI.ShowDialogue(permanentMagicianDialogue);
    }

    public void OnInspectBeam()
    {
        if (!isCompleted)
        {
            solutionThoughtBubble.SetActive(true);
            Debug.Log("Player is thinking of a solution...");
        }
    }

    public void PulleySlotted()
    {
        hasPulley = true;
        //pulleyVisualOnBubble.SetActive(false); 
        Debug.Log("PulleySlotted()");
        CheckCompletion();
    }

    public void RopeSlotted(GameObject placedRope)
    {
        hasRope = true;
        physicalRopeInSocket = placedRope; // Store reference to the barn rope
        
        //ropeVisualOnBubble.SetActive(false); 
        
        // Activate the "Strung" visual that is a child of the mill
        if (strungRopeVisual != null)
            strungRopeVisual.SetActive(true); 
            
        Debug.Log("RopeSlotted()");
        if (physicalRopeInSocket != null) Destroy(physicalRopeInSocket); // Remove barn rope
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
        //thoughtBubble.SetActive(false);
        Debug.Log("CompleteQuest()");
        StartCoroutine(MoveBeamSequence());
    }

    private IEnumerator MoveBeamSequence()
    {
        // Set Baker to pulling state
        if (bakerAnimator != null) bakerAnimator.SetBool("isPulling", true);

        // Record starting positions for all moving parts
        Vector3 beamStart = ironBeam.transform.position;
        Vector3 bakerStart = bakerTransform.position;
        Vector3 hookStart = hookTransform.position;
        Vector3 ropeStart = strungRopeVisual.transform.position;

        Vector3 offset = new Vector3(3f, 0, 0); // Total distance to move
        float duration = 2.0f;
        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float percent = elapsed / duration;
            float curve = Mathf.SmoothStep(0, 1, percent); // Mechanical feel

            // Calculate the new shared offset for this frame
            Vector3 currentOffset = offset * curve;

            // Move all parts together
            ironBeam.transform.position = beamStart + currentOffset;
            bakerTransform.position = bakerStart + currentOffset;
            hookTransform.position = hookStart + currentOffset;
            strungRopeVisual.transform.position = ropeStart + currentOffset;

            yield return null; 
        }

        // Stop Baker animation
        if (bakerAnimator != null) bakerAnimator.SetBool("isPulling", false);

        // 1. Baker is happy
        bakerSadFace.SetActive(false);
        bakerHappyFace.SetActive(true);

        // 2. TRIGGER THE AUTOMATIC DIALOGUE
        dialogueUI.ShowDialogue(miracleDialogue);
        
        // 3. Start a timer to auto-close, but PlayerInteraction can still skip it
        StartCoroutine(AutoCloseDialogue(5f));

        // --- CLEANUP ---
        if (strungRopeVisual != null) Destroy(strungRopeVisual);         
        if (ironBeam != null) Destroy(ironBeam);                         
        // Hook and Baker remain in their new positions

        Instantiate(flourPrefab, flourSpawnPoint.position, Quaternion.identity);
        Debug.Log("Quest 1 Complete: All items moved and beam cleared!");
    }

    public void HideVision()
    {
        if (!isCompleted) solutionThoughtBubble.SetActive(false);
    }

    private IEnumerator AutoCloseDialogue(float duration)
    {
        float elapsed = 0;
        while (elapsed < duration)
        {
            // If the player hits interact, the DialogueUIController will handle 
            // closing it, so we just check if it's still active
            if (!dialogueUI.gameObject.activeInHierarchy) yield break;
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        dialogueUI.gameObject.SetActive(false);
    }
}