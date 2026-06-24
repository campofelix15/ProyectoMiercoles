using UnityEngine;

public class BaseWeapon : IWeapon
{
    private GameObject _bulletPrefab;
    private Transform _firePoint;
    private float _bulletSpeed;
    private float _bulletLifetime;
    private int _damage;
    private float _knockbackForce;

    public BaseWeapon(GameObject bulletPrefab, Transform firePoint, float bulletSpeed, float bulletLifetime, int damage, float knockbackForce)
    {
        _bulletPrefab = bulletPrefab;
        _firePoint = firePoint;
        _bulletSpeed = bulletSpeed;
        _bulletLifetime = bulletLifetime;
        _damage = damage;
        _knockbackForce = knockbackForce;
    }

    public void Shoot(Vector2 direction)
    {
        FireBullet(direction);
    }

    public int GetDamage()
    {
        return _damage;
    }

    public float GetKnockbackForce()
    {
        return _knockbackForce;
    }

    void FireBullet(Vector2 dir)
    {
        GameObject bullet = ObjectPoolManager.SpawnObject(_bulletPrefab, _firePoint.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().Init(dir, _bulletLifetime, _damage, _knockbackForce);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = dir * _bulletSpeed;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
