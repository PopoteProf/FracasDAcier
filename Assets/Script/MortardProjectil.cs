using UnityEngine;

public class MortardProjectil : Projectile
{
    [SerializeField] private GameObject _prfDebugArea;
    [SerializeField] private float _explosionsRadius;

    protected override void Impact()
    {
        if (_timer < _timeBeforeExplosion) return;
        
        bool hitSomething = Physics.Raycast(transform.position + Vector3.up, Vector3.down, 
            out RaycastHit hit, 10);

        if (hitSomething)
        {
            if (hit.transform.GetComponent<IDamagable>() != null) 
            { 
                hit.transform.GetComponent<IDamagable>().TakeDamage(_damage, hit.point, transform.position - _lastPos);
            }
            
            if( _prfDebugArea)Instantiate(_prfDebugArea, hit.point, Quaternion.identity);
        
            GameObject go = Instantiate(_prfDeath, transform.position, Quaternion.identity);
            // go.transform.up = hit.normal;
            Destroy(gameObject);
        }
        
        // foreach (var coll in Physics.OverlapSphere(hit.point, _explosionsRadius)) {
        //     if (coll.transform.GetComponent<IDamagable>() != null) {
        //         coll.transform.GetComponent<IDamagable>().TakeDamage(_damage, hit.point, transform.position - _lastPos);
        //     }
        // }

        
    }
}