using UnityEngine;

public class GameOverHandler : MonoBehaviour, IObserver
{
    private PlayerHealth playerHealth;

    private void Start()
    {
        // Get the health from the PlayerManager
        playerHealth = SessionController.Instance.PlayerManager.PlayerHealth;
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
        Debug.Log("Game Over triggered by Observer.");
        
        SceneController.Instance
            .NewTransition()
            .Unload(SceneDataBase.Scenes.Match)
            .Unload(SceneDataBase.Scenes.Session)
            .Load(SceneDataBase.Slots.Menu, SceneDataBase.Scenes.MainMenu)
            .WithClearUnusedAssets()
            .WithOverlay()
            .Perfrom();
    }
}
