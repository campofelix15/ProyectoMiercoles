using UnityEngine;

public class CheckpointSystem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        PlayerManager playerManager = collision.GetComponentInParent<PlayerManager>();
        if (playerManager == null)
        {
            playerManager = FindFirstObjectByType<PlayerManager>();
        }

        if (playerManager == null) return;

        playerManager.SaveCheckpoint(transform.position);
        Debug.Log("Llegue al checkpoint!");

        Destroy(gameObject);
    }
}
