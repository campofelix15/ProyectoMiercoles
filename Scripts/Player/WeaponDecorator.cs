using UnityEngine;

public abstract class WeaponDecorator : IWeapon
{
    protected IWeapon wrappedWeapon;

    public WeaponDecorator(IWeapon wrappedWeapon)
    {
        this.wrappedWeapon = wrappedWeapon;
    }

    public virtual void Shoot(Vector2 direction)
    {
        wrappedWeapon.Shoot(direction);
    }

    public virtual int GetDamage()
    {
        return wrappedWeapon.GetDamage();
    }

    public virtual float GetKnockbackForce()
    {
        return wrappedWeapon.GetKnockbackForce();
    }
}
