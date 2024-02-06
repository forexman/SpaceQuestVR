using UnityEngine;

public interface IProjectileFactory
{
    GameObject CreateProjectile(WeaponData weaponData, Transform origin);
}