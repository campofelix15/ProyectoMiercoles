using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Health")]
    PlayerHealth health;

    [Header("Weapon")]
    PlayerWeapon weapon;
    [SerializeField] private GameObject bulletPrefab;
    public GameObject BulletPrefab => bulletPrefab;
    int damage = 1;
    public int Damage => damage;
    int bulletsCount = 1;
    public int BulletsCount => bulletsCount;
    float bulletsSpread = 0f;
    public float BulletSpread => bulletsSpread;
    float bulletSpeed = 10f;
    public float BulletSpeed => bulletSpeed;
    float knockbackForce;
    public float KnockbackForce => knockbackForce;
    [SerializeField] private Transform firePoint;
    public Transform FirePoint => firePoint;

    [Header("Visuals")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float flickerInterval = 0.1f;

    [Header("Movement")]
    PlayerMovement movement;
    float speed;
    float jumpForce;
    Rigidbody2D rb;

    private InputSystem_Actions inputActions;
    public InputSystem_Actions InputActions => inputActions;

    private void Awake()
    {
        inputActions = new();
        rb = GetComponent<Rigidbody2D>();
        if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        
        // Obtenemos referencias locales si están en el mismo objeto
        weapon = GetComponent<PlayerWeapon>();
        health = GetComponent<PlayerHealth>();
        movement = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        // Forzamos la obtención de componentes locales primero
        weapon = GetComponent<PlayerWeapon>();
        health = GetComponent<PlayerHealth>();
        movement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();

        if (health == null)
        {
            var pmLocal = GetComponent<PlayerManager>();
            if (pmLocal != null) health = pmLocal.PlayerHealth;
        }

        if (health == null)
        {
            var pm = FindFirstObjectByType<PlayerManager>();
            if (pm != null) health = pm.PlayerHealth;
        }

        // Si no se encontraron en el objeto local (porque se añaden dinámicamente o están en otro sitio), buscamos en el Manager
        if (weapon == null || health == null)
        {
            var manager = SessionController.Instance?.PlayerManager;
            if (manager != null)
            {
                if (weapon == null) weapon = manager.PlayerWeapon;
                if (health == null) health = manager.PlayerHealth;
            }
        }

        // Verificación de seguridad para evitar NullReferenceException
        if (health == null) Debug.LogError("PlayerHealth no encontrado en el Jugador ni en el PlayerManager!");
        if (weapon == null) Debug.LogError("PlayerWeapon no encontrado en el Jugador ni en el PlayerManager!");

        if (weapon != null)
        {
            weapon.Init(this);
            inputActions.Player.Attack.performed += weapon.OnFire;           
            inputActions.Player.Move.performed += weapon.OnMove;             
            inputActions.Player.AltAttack.performed += weapon.OnAltFire;
        }
        
        inputActions.Enable();
    }

    private void OnEnable()
    {
        // Movido a Start para evitar NullReferenceException con weapon
    }

    private void Update()
    {
        HandleFlicker();
    }

    private void HandleFlicker()
    {
        if (health != null && health.IsInvulnerable)
        {
            float alpha = (Mathf.Floor(Time.time / flickerInterval) % 2 == 0) ? 0.2f : 1.0f;
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }
        else if (spriteRenderer != null && spriteRenderer.color.a < 1.0f)
        {
            Color color = spriteRenderer.color;
            color.a = 1.0f;
            spriteRenderer.color = color;
        }
    }

    private void OnDisable()
    {
        if (weapon != null)
        {
            inputActions.Player.Attack.performed -= weapon.OnFire;           
            inputActions.Player.Move.performed -= weapon.OnMove;             
            inputActions.Player.AltAttack.performed -= weapon.OnAltFire;
        }
        inputActions.Disable();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Player triggered with: " + other.gameObject.name + " on layer: " + other.gameObject.layer);

        if ((1 << other.gameObject.layer & (1 << 8)) != 0 || other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Enemy detected in Trigger!");
            if (health.TakeDamage(1))
            {
                Vector2 knockbackDir = (transform.position - other.transform.position).normalized;
                if (Mathf.Abs(knockbackDir.x) < 0.1f) knockbackDir.x = transform.position.x > other.transform.position.x ? 1 : -1;

                ApplyKnockback(knockbackDir, 10f);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Player collided with: " + collision.gameObject.name + " on layer: " + collision.gameObject.layer);
        
        // Probamos con una detección más flexible para depurar
        // Capa 8 suele ser enemigos. 
        if ((1 << collision.gameObject.layer & (1 << 8)) != 0 || collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Enemy detected!");
            if (health.TakeDamage(1))
            {
                Vector2 knockbackDir = (transform.position - collision.transform.position).normalized;
                if (Mathf.Abs(knockbackDir.x) < 0.1f) knockbackDir.x = transform.position.x > collision.transform.position.x ? 1 : -1;
                
                ApplyKnockback(knockbackDir, 10f);
            }
        }
    }

    public void ApplyKnockback(Vector2 direction, float force)
    {
        rb.linearVelocity = Vector2.zero; // Limpiar velocidad previa para un empuje consistente
        rb.AddForce(direction * force, ForceMode2D.Impulse);
    }



}
