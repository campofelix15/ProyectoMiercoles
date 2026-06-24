using UnityEngine;

public class AudioManagerFacade : MonoBehaviour
{
    [Header("Music Clips")]
    [SerializeField] private AudioClip backgroundMusic;

    [Header("SFX Clips")]
    [SerializeField] private AudioClip shootSFX;
    [SerializeField] private AudioClip hitSFX;
    [SerializeField] private AudioClip enemyDeathSFX;
    [SerializeField] private AudioClip playerHurtSFX;
    [SerializeField] private AudioClip jumpSFX;

    private void OnEnable()
    {
        PlayBackgroundMusic();
    }

    private void OnDisable()
    {
        StopBackgroundMusic();
    }

    public void PlayBackgroundMusic()
    {
        if (AudioManager.Instance != null && backgroundMusic != null)
        {
            AudioManager.Instance.PlayMusic(backgroundMusic);
        }
    }

    public void StopBackgroundMusic()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopMusic();
        }
    }

    public void PlayShootSound()
    {
        if (AudioManager.Instance != null && shootSFX != null)
        {
            AudioManager.Instance.PlaySFX(shootSFX);
        }
    }

    public void PlayHitSound()
    {
        if (AudioManager.Instance != null && hitSFX != null)
        {
            AudioManager.Instance.PlaySFX(hitSFX);
        }
    }

    public void PlayEnemyDeathSound()
    {
        if (AudioManager.Instance != null && enemyDeathSFX != null)
        {
            AudioManager.Instance.PlaySFX(enemyDeathSFX);
        }
    }

    public void PlayPlayerHurtSound()
    {
        if (AudioManager.Instance != null && playerHurtSFX != null)
        {
            AudioManager.Instance.PlaySFX(playerHurtSFX);
        }
    }

    public void PlayJumpSound()
    {
        if (AudioManager.Instance != null && jumpSFX != null)
        {
            AudioManager.Instance.PlaySFX(jumpSFX);
        }
    }

    public void PlaySFXAtPosition(AudioClip clip, Vector3 position)
    {
        if (AudioManager.Instance != null && clip != null)
        {
            AudioManager.Instance.PlaySFXAtPoint(clip, position);
        }
    }
}
