using Unity.Netcode;
using UnityEngine;
public class NetProjectile : NetworkBehaviour {
    
    [SerializeField] protected Rigidbody _rb;
    [SerializeField] protected int _damage;
    [SerializeField] protected GameObject _prfDeath;
    protected Vector3 _lastPos;
    public LayerMask _hitmask ;
    private Vector3 HitPoint;
    
    public virtual void SetUpProjectile(int damage, Vector3 force) {
        _lastPos = transform.position;
        _damage = damage;
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void SetUpProjectileRpc(Vector3 force) {
        _rb.AddForce(force, ForceMode.Impulse);
    }

    protected virtual void Update() {
        if (IsServer) {
            RaycastHit hit;
            if (Physics.Raycast(new Ray(_lastPos, transform.position - _lastPos), out hit,
                    (transform.position - _lastPos).magnitude))
            {
                Impact(hit);
            }
            _lastPos = transform.position;
        }

        transform.forward = _rb.linearVelocity.normalized;
    }

    protected virtual void Impact(RaycastHit hit) {
        
        if (hit.transform.GetComponentInParent<IDamagable>() != null) {
            hit.transform.GetComponentInParent<IDamagable>().TakeDamage(_damage, hit.point, transform.position - _lastPos);
        }
        PlayImpactVFXRpc(hit.point, hit.normal);
        NetworkObject.Despawn();
    }

    [Rpc(SendTo.ClientsAndHost)]
    protected virtual void PlayImpactVFXRpc(Vector3 position, Vector3 normal) {
        GameObject go = Instantiate(_prfDeath, position, Quaternion.identity);
        go.transform.up = normal;
    }
    
}