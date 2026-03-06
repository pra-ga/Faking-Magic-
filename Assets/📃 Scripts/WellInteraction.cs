using UnityEngine;

public class WellInteraction : MonoBehaviour
{
    [Header("Quest References")]
    [SerializeField] private GameObject physicalPulley; // The actual CarryableItem in the scene
    [SerializeField] private GameObject magnetVisual;   // A static sprite of a magnet on the well
    [SerializeField] private GameObject decorativePulley;
    [SerializeField] private GameObject magnetThoughtBubble;
    
    [Header("Effects")]
    [SerializeField] private ParticleSystem clickParticles; // Optional "spark" effect
    [SerializeField] private AudioClip clinkSound;          // Optional "metal click" sound

    private void Awake()
    {
        // Ensure the pulley is hidden and non-interactable at the start
        if (physicalPulley != null)
            physicalPulley.SetActive(false);

        if (magnetVisual != null)
            magnetVisual.SetActive(false);

        // Ensure the decorative pulley is visible at the start
        if (decorativePulley != null)
            decorativePulley.SetActive(true);
    }

    // This is the method you invoke in the QuestSocket's OnItemPlaced() event
    public void UnlockPulley()
    {
        Debug.Log("Magnet placed! The Pulley is clicking free...");

        // 1. Show the magnet stuck to the well
        if (magnetVisual != null)
            magnetVisual.SetActive(true);
        
        // Hide the "fake" pulley and show the "real" one
        if (decorativePulley != null)
            decorativePulley.SetActive(false);

        // 2. Enable the Pulley so the player can now see and pick it up
        if (physicalPulley != null)
        {
            physicalPulley.SetActive(true);
            
            // Optional: Make it "pop" out of the well slightly using a tiny Coroutine
            StartCoroutine(PopPulleyOut());
        }

        // 3. Play feedback effects
        if (clinkSound != null)
            AudioSource.PlayClipAtPoint(clinkSound, transform.position);
            
        if (clickParticles != null)
            clickParticles.Play();
    }

    private System.Collections.IEnumerator PopPulleyOut()
    {
        Vector3 startPos = physicalPulley.transform.position;
        Vector3 endPos = startPos + new Vector3(0.5f, 0.5f, 0); // Pop up and to the right
        float elapsed = 0;
        float duration = 0.5f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float percent = elapsed / duration;
            // Use an arc-like movement
            physicalPulley.transform.position = Vector3.Lerp(startPos, endPos, Mathf.Sin(percent * Mathf.PI / 2));
            yield return null;
        }
    }

    public void ShowWellVision()
    {
        // Only show if the pulley hasn't been unlocked yet
        if (magnetThoughtBubble != null && !physicalPulley.activeInHierarchy)
        {
            magnetThoughtBubble.SetActive(true);
        }
    }

    public void HideWellVision()
    {
        if (magnetThoughtBubble != null)
            magnetThoughtBubble.SetActive(false);
    }
}