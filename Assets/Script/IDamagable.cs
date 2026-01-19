using UnityEngine;

public interface IDamagable 
{
    public void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitNormal);
}