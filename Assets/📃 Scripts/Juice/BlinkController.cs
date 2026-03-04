using UnityEngine;
using System.Collections;

public class BlinkController : MonoBehaviour
{
    [Header("Sprite Settings")]
    [SerializeField] private Sprite openEyes;   // Your normal eye sprite
    [SerializeField] private Sprite closedEyes; // Your blink/flat-line sprite
    
    [Header("Timing Settings")]
    [SerializeField] private float blinkDuration = 0.15f; // How long eyes stay shut
    [SerializeField] private float minBlinkInterval = 2f; // Minimum time between blinks
    [SerializeField] private float maxBlinkInterval = 6f; // Maximum time between blinks

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = openEyes;
    }

    private void Start()
    {
        // Start the infinite blinking loop
        StartCoroutine(BlinkRoutine());
    }

    private IEnumerator BlinkRoutine()
    {
        while (true)
        {
            // Wait for a random amount of time before blinking
            float waitTime = Random.Range(minBlinkInterval, maxBlinkInterval);
            yield return new WaitForSeconds(waitTime);

            // Swap to closed eyes
            spriteRenderer.sprite = closedEyes;

            // Wait for the brief blink duration
            yield return new WaitForSeconds(blinkDuration);

            // Swap back to open eyes
            spriteRenderer.sprite = openEyes;
        }
    }
}