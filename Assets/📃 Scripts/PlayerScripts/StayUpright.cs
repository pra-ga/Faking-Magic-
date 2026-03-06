using UnityEngine;

public class StayUpright : MonoBehaviour
{
    private Vector3 initialScale;

    void Awake()
    {
        // Store the original local scale of the image
        initialScale = transform.localScale;
    }

    void LateUpdate()
    {
        // We check the "Lossy Scale" (Global Scale)
        // If the total scale on X is negative, the parent (player) is flipped.
        if (transform.parent.lossyScale.x < 0)
        {
            // Reverse the local X to counteract the parent flip
            transform.localScale = new Vector3(-initialScale.x, initialScale.y, initialScale.z);
        }
        else
        {
            // Stay normal
            transform.localScale = initialScale;
        }
    }
}