using UnityEngine;

public interface IWeapon
{
    void Shoot(Vector2 direction);
    int GetDamage();
    float GetKnockbackForce();
}
