using UnityEngine;

public interface IProjectile
{
    void Launch(Vector3 direction, float speed);
    void OnHit(GameObject hitObject);
}
