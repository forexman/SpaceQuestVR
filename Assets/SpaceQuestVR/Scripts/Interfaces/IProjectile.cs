using UnityEngine;

public interface IProjectile
{
    public void Initialize(Vector3 direction, float speed, int damage);
}
