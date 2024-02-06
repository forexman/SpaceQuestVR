using UnityEngine;

public class ProjectileFactory : MonoBehaviour, IProjectileFactory
{
    public GameObject CreateProjectile(WeaponData weaponData, Transform origin)
    {
        GameObject projectileObject = PoolManager.Instance.GetFromPool(weaponData.projectilePrefab.name);
        if (projectileObject != null)
        {
            projectileObject.transform.SetPositionAndRotation(origin.position, origin.rotation);
            IProjectile projectile = projectileObject.GetComponent<IProjectile>();
            projectile?.Initialize(origin.forward, weaponData.projectileSpeed, weaponData.projectileDamage);
        }
        return projectileObject;
    }
}
