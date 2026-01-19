using UnityEngine;

public class Projectile : MonoBehaviour {
    [SerializeField] protected Rigidbody _rb;
    [SerializeField] protected int _damage;
    [SerializeField] protected GameObject _prfDeath;
    protected Vector3 _lastPos;

    public virtual void SetUpProjectile(int damage, Vector3 force) 
    {
        _lastPos = transform.position;
        _damage = damage;
        _rb.AddForce(force, ForceMode.Impulse);
    }

    protected virtual void Update() {
        RaycastHit hit;
        if (Physics.Raycast(new Ray(_lastPos, transform.position - _lastPos), out hit,
                (transform.position - _lastPos).magnitude)){
            Impact(hit);
        }
        transform.forward = _rb.linearVelocity.normalized;
    }

    protected virtual void Impact(RaycastHit hit)
    {
        
        if (hit.transform.GetComponent<IDamagable>() != null) {
            hit.transform.GetComponent<IDamagable>().TakeDamage(_damage, hit.point, transform.position - _lastPos);
        }
        GameObject go = Instantiate(_prfDeath, transform.position, Quaternion.identity);
        go.transform.up = hit.normal;
        Destroy(gameObject);
    }
}