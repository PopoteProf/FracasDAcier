using System;
using Unity.Netcode;
using UnityEngine;

public class NetMortar : NetWeapon {
    [SerializeField, ColorUsage(true, true)] private Color _defaultLazerColor;
    [SerializeField, ColorUsage(true, true)] private Color _fireLazerColor;
    [SerializeField] private int _damage =4;
    [SerializeField] private NetProjectile _prfProjectile;
    [SerializeField] private float _projectileSpeed = 30;
    
    void Start() {
        _aimLineRenderer.startColor =_defaultLazerColor;
        _aimLineRenderer.endColor =_defaultLazerColor;
    }
  
    [Rpc (SendTo.Server)]
    private void FireProjectileRpc()
    {

        ulong clientId = UInt64.MaxValue;
        PlayFireVFXRpc();
        NetProjectile projectile = Instantiate(_prfProjectile, _firePoint.position, Quaternion.identity);
        projectile.transform.forward = _firePoint.forward;
        projectile.SetUpProjectile(_damage, _firePoint.forward*_projectileSpeed);
        
        projectile.NetworkObject.SpawnWithOwnership(clientId);
        projectile.NetworkObject.Spawn();
        projectile.SetUpProjectileRpc(_firePoint.forward*_projectileSpeed);
    }
    [Rpc(SendTo.ClientsAndHost)]
    private void PlayFireVFXRpc() {
        if (_prfMuzzleFire != null) Instantiate(_prfMuzzleFire, _firePoint.position,_firePoint.rotation);
        if( _fireImpulseSource!=null)_fireImpulseSource.GenerateImpulse();
    }

    public override void StartClick() {
        FireProjectileRpc();
        _aimLineRenderer.startColor =_fireLazerColor;
        _aimLineRenderer.endColor =_fireLazerColor;
    }

    public override void StopClick() {
        _aimLineRenderer.startColor = _defaultLazerColor;
        _aimLineRenderer.endColor = _defaultLazerColor;
    }
}