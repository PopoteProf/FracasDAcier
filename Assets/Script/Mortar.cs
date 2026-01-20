using System;
using Unity.Mathematics;
using UnityEngine;

public class Mortar : Weapon {
    [SerializeField, ColorUsage(true, true)] private Color _defaultLazerColor;
    [SerializeField, ColorUsage(true, true)] private Color _fireLazerColor;
    [SerializeField] private int _damage =4;
    [SerializeField] private Projectile _prfProjectile;
    [SerializeField] private float _projectileSpeed = 30;
    
    void Start() {
        _aimLineRenderer.startColor =_defaultLazerColor;
        _aimLineRenderer.endColor =_defaultLazerColor;
    }
  
    private void FireProjectile(object sender, EventArgs e) {
        if (_prfMuzzleFire != null) Instantiate(_prfMuzzleFire, _firePoint.position,_firePoint.rotation);
        if( _fireImpulseSource!=null)_fireImpulseSource.GenerateImpulse();
        
        Projectile projectile = Instantiate(_prfProjectile, _firePoint.position, Quaternion.identity);
        projectile.transform.forward = _firePoint.forward;
        projectile.SetUpProjectile(_damage, _firePoint.forward*_projectileSpeed);
    }

    public override void StartClick() {
        FireProjectile(this, null);
        _aimLineRenderer.startColor =_fireLazerColor;
        _aimLineRenderer.endColor =_fireLazerColor;
    }

    public override void StopClick() {
        _aimLineRenderer.startColor = _defaultLazerColor;
        _aimLineRenderer.endColor = _defaultLazerColor;
    }
}