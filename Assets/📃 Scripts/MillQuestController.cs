using System.Collections;
using UnityEngine;

public class MillQuestController : MonoBehaviour
{
    public GameObject thoughtBubble;
    public GameObject pulleyVisualOnBubble;
    public GameObject ropeVisualOnBubble;
    
    [Header("Quest Objects")]
    public GameObject ironBeam;
    public GameObject flourPrefab;
    public Transform flourSpawnPoint;

    [Header("Visual Feedback")]
    public GameObject strungRopeVisual; // The rope looping through the pulley
    private GameObject physicalRopeInSocket; // The temporary rope found in the barn

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
        pulleyVisualOnBubble.SetActive(false); 
        Debug.Log("PulleySlotted()");
        CheckCompletion();
    }

    public void RopeSlotted(GameObject placedRope)
    {
        hasRope = true;
        physicalRopeInSocket = placedRope; // Store reference to the barn rope
        
        ropeVisualOnBubble.SetActive(false); 
        
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
        thoughtBubble.SetActive(false);
        Debug.Log("CompleteQuest()");
        StartCoroutine(MoveBeamSequence());
    }

    private IEnumerator MoveBeamSequence()
    {
        Vector3 startPos = ironBeam.transform.position;
        Vector3 endPos = startPos + new Vector3(3f, 0, 0); 
        float duration = 2.0f;
        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float percent = elapsed / duration;
            float curve = Mathf.SmoothStep(0, 1, percent);
            ironBeam.transform.position = Vector3.Lerp(startPos, endPos, curve);
            yield return null; 
        }

        ironBeam.transform.position = endPos;

        // --- CLEANUP ---
        if (strungRopeVisual != null) Destroy(strungRopeVisual);         // Remove strung visual
        if (ironBeam != null) Destroy(ironBeam);                         // Remove the beam
        // Pulley is preserved as requested

        Instantiate(flourPrefab, flourSpawnPoint.position, Quaternion.identity);
        Debug.Log("Quest 1 Complete: Baker is happy!");
    }

    public void HideVision()
    {
        if (!isCompleted) thoughtBubble.SetActive(false);
    }
}