using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetMortardProjectil : NetProjectile {
    [SerializeField] private GameObject _prfDebugArea;
    [SerializeField] private float _explosionsRadius;

    protected override void Impact(RaycastHit hit) {
        Debug.Log("PlayHit");
        List<IDamagable> damaged = new List<IDamagable>();
        foreach (var coll in Physics.OverlapSphere(hit.point, _explosionsRadius)) {
            if (coll.transform.GetComponent<IDamagable>() != null) {
                IDamagable iDamagable = coll.transform.GetComponent<IDamagable>();
                if (damaged.Contains(iDamagable)) continue;
                iDamagable.TakeDamage(_damage, hit.point, transform.position - _lastPos);
                damaged.Add(iDamagable);
            }
        }
        PlayImpactVFXRpc(hit.point, hit.normal);
        NetworkObject.Despawn();
    }
    
    [Rpc(SendTo.ClientsAndHost)]
    protected override void PlayImpactVFXRpc(Vector3 position, Vector3 normal) {
        GameObject go = Instantiate(_prfDeath, position, Quaternion.identity);
        go.transform.up = normal;
        if( _prfDebugArea)Instantiate(_prfDebugArea, position, Quaternion.identity);
        
    }
}