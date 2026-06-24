using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour, IObserver
{
    [Header("Hearts UI (opcional)")]
    public GameObject heartsPrefab;

    [Header("Bar UI (opcional)")]
    [SerializeField] private Image fillImage;

    private PlayerHealth playerHealth;
    private List<HealthHeart> hearts = new List<HealthHeart>();
    private bool isBound;
    private int lastCurrentHealth = -1;
    private int lastMaxHealth = -1;

    private void OnEnable()
    {
        TryBind();
    }

    private void Update()
    {
        if (!isBound)
        {
            TryBind();
            return;
        }

        if (playerHealth != null &&
            (playerHealth.CurrentHealth != lastCurrentHealth || playerHealth.MaxHealth != lastMaxHealth))
        {
            RefreshUI();
        }
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
        RefreshUI();
    }

    private void RefreshUI()
    {
        if (playerHealth == null) return;

        if (fillImage != null)
        {
            if (playerHealth.MaxHealth <= 0) fillImage.fillAmount = 0f;
            else fillImage.fillAmount = Mathf.Clamp01((float)playerHealth.CurrentHealth / playerHealth.MaxHealth);
        }

        if (heartsPrefab != null)
        {
            Drawhearts();
        }

        lastCurrentHealth = playerHealth.CurrentHealth;
        lastMaxHealth = playerHealth.MaxHealth;
    }

    private void TryBind()
    {
        if (isBound) return;

        if (fillImage == null)
        {
            Transform fillTransform = transform.Find("Fill");
            if (fillTransform != null)
            {
                fillImage = fillTransform.GetComponent<Image>();
            }
        }

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerHealth = playerObject.GetComponent<PlayerHealth>();
            if (playerHealth == null)
            {
                PlayerManager playerManagerOnPlayer = playerObject.GetComponent<PlayerManager>();
                if (playerManagerOnPlayer != null)
                {
                    playerHealth = playerManagerOnPlayer.PlayerHealth;
                }
            }
        }

        if (playerHealth == null &&
            SessionController.Instance != null &&
            SessionController.Instance.PlayerManager != null &&
            SessionController.Instance.PlayerManager.gameObject.CompareTag("Player"))
        {
            playerHealth = SessionController.Instance.PlayerManager.PlayerHealth;
        }

        if (playerHealth == null) return;

        playerHealth.Attach(this);
        isBound = true;
        RefreshUI();
    }

    public void Drawhearts()
    {
        if (heartsPrefab == null) return;

        Clearhearts();

        //cuantos corazones hay en total

        float maxHealthRemainder = playerHealth.MaxHealth % 2;
        int heartssToMake = (int)((playerHealth.MaxHealth / 2) + maxHealthRemainder);
        for (int i = 0; i < heartssToMake; i++)
        {
            CreateEmptyhearts();
        }

        for (int i = 0; i < hearts.Count; i++)
        {
            int heartssStatusRemainder = Mathf.Clamp(playerHealth.CurrentHealth - (i * 2), 0, 2);
            hearts[i].SetHeartImage((HeartStatus)heartssStatusRemainder);
        }

    }


    public void CreateEmptyhearts()
    {
        if (heartsPrefab == null) return;

        GameObject newhearts = Instantiate(heartsPrefab);
        newhearts.transform.SetParent(transform, false);

        HealthHeart heartsComponent = newhearts.GetComponent<HealthHeart>();
        heartsComponent.SetHeartImage(HeartStatus.Empty);
        hearts.Add(heartsComponent);
    }

    public void Clearhearts()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        hearts = new List<HealthHeart>();

    }



}
