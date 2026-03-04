using UnityEngine;

public class WellInteraction : MonoBehaviour
{
    [Header("Quest References")]
    [SerializeField] private GameObject physicalPulley; // The actual CarryableItem in the scene
    [SerializeField] private GameObject magnetVisual;   // A static sprite of a magnet on the well
    
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
    }

    // This is the method you invoke in the QuestSocket's OnItemPlaced() event
    public void UnlockPulley()
    {
        Debug.Log("Magnet placed! The Pulley is clicking free...");

        // 1. Show the magnet stuck to the well
        if (magnetVisual != null)
            magnetVisual.SetActive(true);

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
}