using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapon : MonoBehaviour
{
    private IWeapon _weapon;
    private float bulletSpread;
    private int bulletsCount;

    private InputSystem_Actions inputActions;

    private Vector2 movement;
    private Vector2 lastDirection = Vector2.right; // default

    [SerializeField] private bool hasCriticalHit = false;

    public void Init(PlayerController player)
    {                                                                   
        inputActions = player.InputActions;
        bulletSpread = player.BulletSpread;
        bulletsCount = player.BulletsCount;

        IWeapon baseWeapon = new BaseWeapon(
            player.BulletPrefab, 
            player.FirePoint, 
            player.BulletSpeed, 
            0.5f, 
            player.Damage, 
            player.KnockbackForce
        );

        if (hasCriticalHit)
        {
            _weapon = new CriticalHitWeaponDecorator(baseWeapon);
        }
        else
        {
            _weapon = baseWeapon;
        }
    }

    public void OnAltFire(InputAction.CallbackContext ctx)
    {
        ShootSpread();
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        movement = ctx.ReadValue<Vector2>();

        if (movement != Vector2.zero)
        {
            lastDirection = movement.normalized;
        }
    }

    public void OnFire(InputAction.CallbackContext ctx)
    {
        ShootNormal();
    }

    void ShootNormal()
    {
        Vector2 baseDir = (movement != Vector2.zero)
        ? movement.normalized
        : lastDirection;

        _weapon.Shoot(baseDir);
    }

    void ShootSpread()
    {
        Vector2 baseDir = (movement != Vector2.zero)
            ? movement.normalized
            : lastDirection;

        _weapon.Shoot(baseDir);
        _weapon.Shoot(Rotate(baseDir, bulletSpread));
        _weapon.Shoot(Rotate(baseDir, -bulletSpread));
    }

    Vector2 Rotate(Vector2 direction, float angle)
    {
        return Quaternion.Euler(0, 0, angle) * direction;
    }
}