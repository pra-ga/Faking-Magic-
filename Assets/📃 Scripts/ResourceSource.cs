using UnityEngine;

public class ResourceSource : MonoBehaviour
{
    public GameObject itemToGivePrefab;
    public string requiredToolName = ""; 
    public float harvestDuration = 0.5f;
    [SerializeField] GameObject resourceSpawnPoint;
    Collider2D collider2D;

    private bool isHarvested = false; // Prevents multiple spawns

    public void OnHarvest(PlayerInteraction player)
    {
        if (isHarvested) return; // Stop if already pulled

        if (!string.IsNullOrEmpty(requiredToolName))
        {
            if (player.currentItem == null || player.currentItem.itemName != requiredToolName)
            {
                Debug.Log($"You need a {requiredToolName} to get this!");
                return;
            }
        }

        isHarvested = true; // Mark as done immediately to block rapid clicking
        player.GetComponent<PlayerMovement>().PlayDigAnimation(harvestDuration);
        
        Invoke(nameof(SpawnItem), harvestDuration);
    }

    private void SpawnItem()
    {
        collider2D = GetComponent<Collider2D>();
        Instantiate(itemToGivePrefab, new Vector2 (resourceSpawnPoint.transform.position.x, resourceSpawnPoint.transform.position.y), Quaternion.identity);
        
        // Optional: Disable the visual Reeds so the player knows it's empty
        collider2D.enabled = false; 
    }
}