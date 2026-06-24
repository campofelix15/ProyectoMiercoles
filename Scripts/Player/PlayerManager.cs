using System;
using System.IO;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Serializable]
    private class PlayerCheckpointMemento
    {
        public Vector3 respawnPosition;
        public int savedHealth;
    }

    [Serializable]
    private class CheckpointInfoData
    {
        public Vector3 respawnPosition;
        public int savedHealth;
    }

    [SerializeField] private int initialHealthPoints = 10;
    private PlayerHealth playerHealth;
    private PlayerMovement playerMovement;
    private PlayerWeapon playerWeapon;
    private Rigidbody2D playerRigidbody;
    private PlayerCheckpointMemento currentCheckpoint;
    private string checkpointDirectoryPath;
    private string checkpointFilePath;

    public PlayerHealth PlayerHealth => playerHealth;
    public PlayerMovement PlayerMovement => playerMovement;
    public PlayerWeapon PlayerWeapon => playerWeapon;
    public int InitialHealthPoints => initialHealthPoints;

    private void Awake()
    {
        if (SessionController.Instance != null)
        {
            SessionController.Instance.PlayerManager = this;
        }
        
        playerWeapon = GetComponent<PlayerWeapon>();
        if (playerWeapon == null) playerWeapon = gameObject.AddComponent<PlayerWeapon>();

        playerHealth = GetComponent<PlayerHealth>();
        if (playerHealth == null) playerHealth = gameObject.AddComponent<PlayerHealth>();
        
        playerHealth.Initialize(initialHealthPoints);

        playerMovement = GetComponent<PlayerMovement>();
        playerRigidbody = GetComponent<Rigidbody2D>();

        checkpointDirectoryPath = Path.Combine(Application.persistentDataPath, "Serialization");
        checkpointFilePath = Path.Combine(checkpointDirectoryPath, "CheckpointInfo.json");

        currentCheckpoint = new PlayerCheckpointMemento
        {
            respawnPosition = transform.position,
            savedHealth = initialHealthPoints
        };

        LoadCheckpointFromJson();

        if (GetComponent<GameOverHandler>() == null) gameObject.AddComponent<GameOverHandler>();
    }

    public void SaveCheckpoint(Vector3 checkpointPosition)
    {
        currentCheckpoint = new PlayerCheckpointMemento
        {
            respawnPosition = checkpointPosition,
            savedHealth = playerHealth != null ? playerHealth.CurrentHealth : initialHealthPoints
        };

        SaveCheckpointToJson();
    }

    public void RespawnFromCheckpoint()
    {
        LoadCheckpointFromJson();
        EnsureCheckpointExists();

        transform.position = currentCheckpoint.respawnPosition;

        if (playerRigidbody != null)
        {
            playerRigidbody.linearVelocity = Vector2.zero;
            playerRigidbody.angularVelocity = 0f;
        }

        if (playerHealth != null)
        {
            playerHealth.RestoreHealth(currentCheckpoint.savedHealth);
        }

        CameraLeftBorder cameraLeftBorder = FindFirstObjectByType<CameraLeftBorder>();
        if (cameraLeftBorder != null)
        {
            cameraLeftBorder.SnapToPlayer();
        }

        CameraFollowWithBorder cameraFollow = FindFirstObjectByType<CameraFollowWithBorder>();
        if (cameraFollow != null)
        {
            cameraFollow.SnapToPlayer();
        }

        gameObject.SetActive(true);
    }

    private void EnsureCheckpointExists()
    {
        if (currentCheckpoint != null) return;

        currentCheckpoint = new PlayerCheckpointMemento
        {
            respawnPosition = transform.position,
            savedHealth = playerHealth != null ? playerHealth.CurrentHealth : initialHealthPoints
        };
    }

    private void SaveCheckpointToJson()
    {
        EnsureCheckpointExists();

        if (!Directory.Exists(checkpointDirectoryPath))
        {
            Directory.CreateDirectory(checkpointDirectoryPath);
        }

        CheckpointInfoData data = new CheckpointInfoData
        {
            respawnPosition = currentCheckpoint.respawnPosition,
            savedHealth = currentCheckpoint.savedHealth
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(checkpointFilePath, json);
    }

    private void LoadCheckpointFromJson()
    {
        if (!File.Exists(checkpointFilePath))
        {
            SaveCheckpointToJson();
            return;
        }

        string json = File.ReadAllText(checkpointFilePath);
        if (string.IsNullOrWhiteSpace(json))
        {
            return;
        }

        CheckpointInfoData data = JsonUtility.FromJson<CheckpointInfoData>(json);
        if (data == null) return;

        currentCheckpoint = new PlayerCheckpointMemento
        {
            respawnPosition = data.respawnPosition,
            savedHealth = data.savedHealth <= 0 ? initialHealthPoints : data.savedHealth
        };
    }

    //considerar un constructor para las clases del player en caso de querer eliminar monobehaviours
    //player manager deberia ir en player y/o el player en session?

    //Usar el manager para las mejoras 


}
