using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, ISubject
{
    [SerializeField] private int maxHealth = 10;
    private int currentHealth;

    [SerializeField] private float immunityDuration = 1.5f;
    private float immunityTimer;
    public bool IsInvulnerable => immunityTimer > 0;

    private List<IObserver> observers = new List<IObserver>();

    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (immunityTimer > 0)
        {
            immunityTimer -= Time.deltaTime;
        }
    }

    public void Initialize(int initialHealth)
    {
        maxHealth = initialHealth;
        currentHealth = maxHealth;
    }

    public void Attach(IObserver observer)
    {
        if (!observers.Contains(observer))
        {
            observers.Add(observer);
        }
    }

    public void Detach(IObserver observer)
    {
        if (observers.Contains(observer))
        {
            observers.Remove(observer);
        }
    }

    public void Notify()
    {
        foreach (var observer in observers)
        {
            observer.OnNotify();
        }
    }

    public bool TakeDamage(int damage)
    {
        if (IsInvulnerable) return false;

        currentHealth -= damage;
        Debug.Log("Player took damage. Current health: " + currentHealth);
        
        immunityTimer = immunityDuration;
        Notify();
        return true;
    }

    public void GainHealth(int heal)
    {
        currentHealth = Mathf.Min(currentHealth + heal, maxHealth);
        Notify();
    }

    public void RestoreFullHealth()
    {
        currentHealth = maxHealth;
        immunityTimer = 0f;
        Notify();
    }

    public void RestoreHealth(int health)
    {
        currentHealth = Mathf.Clamp(health, 1, maxHealth);
        immunityTimer = 0f;
        Notify();
    }
}
