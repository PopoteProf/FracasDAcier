using System;
using Unity.Netcode;
using UnityEngine;

public class NetMiniGun : NetWeapon
{
    [SerializeField, ColorUsage(true, true)] private Color _defaultLazerColor;
    [SerializeField, ColorUsage(true, true)] private Color _fireLazerColor;
    [SerializeField] private int _damage =2;
    [SerializeField] private NetProjectile _prfProjectile;
    [SerializeField] private float _projectileSpeed = 50;
    [SerializeField] private float _fireRate =3;
    
    private PopoteTimer _fireTimer;
    
    void Awake() {
        _aimLineRenderer.startColor =_defaultLazerColor;
        _aimLineRenderer.endColor =_defaultLazerColor;
        _fireTimer = new PopoteTimer(1f/_fireRate);
        _fireTimer.OnTimerEnd += SendFire;
    }
    protected override void Update() {
        _fireTimer.UpdateTimer();
        base.Update();
        
    }

    private void SendFire(object sender, EventArgs e) {
        FireProjectileRpc();
        _fireTimer.Play();
    }
    [Rpc(SendTo.Server)]
    private void FireProjectileRpc() {
        PlayFireVFXRpc();
        NetProjectile projectile = Instantiate(_prfProjectile, _firePoint.position, Quaternion.identity);
        projectile.transform.forward = _firePoint.forward;
        projectile.SetUpProjectile(_damage, _firePoint.forward*_projectileSpeed);
        projectile.NetworkObject.Spawn();
        projectile.SetUpProjectileRpc(_firePoint.forward*_projectileSpeed);
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void PlayFireVFXRpc() {
        if (_prfMuzzleFire != null) Instantiate(_prfMuzzleFire, _firePoint.position,_firePoint.rotation);
        if( _fireImpulseSource!=null)_fireImpulseSource.GenerateImpulse();
    }

    public override void StartClick() {
        _fireTimer.Play();
        FireProjectileRpc();
        _aimLineRenderer.startColor =_fireLazerColor;
        _aimLineRenderer.endColor =_fireLazerColor;
    }

    public override void StopClick() {
        _fireTimer.Pause();
        _aimLineRenderer.startColor = _defaultLazerColor;
        _aimLineRenderer.endColor = _defaultLazerColor;
    }
}