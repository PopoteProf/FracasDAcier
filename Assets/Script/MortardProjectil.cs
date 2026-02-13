using UnityEngine;
using UnityEngine.VFX;

public class MortardProjectil : Projectile
{
    [SerializeField] private VisualEffect _vfxSmoke;
    [SerializeField] private GameObject _prfDebugArea;
    [SerializeField] private float _explosionsRadius;

    private bool _alreadyPlayedVFX = false;

    protected override void Impact()
    {
        if (_timer < _timeBeforeExplosion) return;
        
        bool hitSomething = Physics.Raycast(transform.position + Vector3.up, Vector3.down, 
            out RaycastHit hit, 10);

        if (hitSomething)
        {
            if (!_alreadyPlayedVFX)
            {
                if (hit.transform.GetComponent<IDamagable>() != null) 
                { 
                    hit.transform.GetComponent<IDamagable>().TakeDamage(_damage, hit.point, transform.position - _lastPos);
                }
            
                if( _prfDebugArea)Instantiate(_prfDebugArea, hit.point, Quaternion.identity);
        
                GameObject go = Instantiate(_prfDeath, transform.position, Quaternion.identity);
                
                _alreadyPlayedVFX = true;
            }
            // go.transform.up = hit.normal;
            _vfxSmoke.Stop();
            Destroy(gameObject, 1.5f);
        }
        
        // foreach (var coll in Physics.OverlapSphere(hit.point, _explosionsRadius)) {
        //     if (coll.transform.GetComponent<IDamagable>() != null) {
        //         coll.transform.GetComponent<IDamagable>().TakeDamage(_damage, hit.point, transform.position - _lastPos);
        //     }
        // }

        
    }
}