using UnityEngine;

public class CriticalHitWeaponDecorator : WeaponDecorator
{
    private float _spreadAngle = 5f;

    public CriticalHitWeaponDecorator(IWeapon wrappedWeapon) : base(wrappedWeapon)
    {
    }

    public override void Shoot(Vector2 direction)
    {
        wrappedWeapon.Shoot(direction);
        wrappedWeapon.Shoot(Rotate(direction, _spreadAngle));
    }

    Vector2 Rotate(Vector2 direction, float angle)
    {
        return Quaternion.Euler(0, 0, angle) * direction;
    }
}
