using UnityEngine;

public class GameOverHandler : MonoBehaviour, IObserver
{
    private PlayerHealth playerHealth;
    private PlayerManager playerManager;

    private void Start()
    {
        playerManager = GetComponent<PlayerManager>();
        if (playerManager == null && SessionController.Instance != null)
        {
            playerManager = SessionController.Instance.PlayerManager;
        }

        if (playerManager == null) return;

        playerHealth = playerManager.PlayerHealth;
        playerHealth.Attach(this);
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.Detach(this);
        }
    }

    public void OnNotify()
    {
        if (playerHealth.CurrentHealth <= 0)
        {
            TriggerGameOver();
        }
    }

    private void TriggerGameOver()
    {
        if (playerManager != null)
        {
            playerManager.RespawnFromCheckpoint();
            return;
        }

        Debug.Log("Game Over triggered by Observer.");
    }
}
