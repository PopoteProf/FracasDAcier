using System;
using UnityEngine;
using UnityEngine.VFX;

public class Mortar : Weapon {
    [SerializeField, ColorUsage(true, true)] private Color _defaultLazerColor;
    [SerializeField, ColorUsage(true, true)] private Color _fireLazerColor;
    [SerializeField] private int _damage =4;
    [SerializeField] private Projectile _prfProjectile;
    [SerializeField] private float _projectileSpeed = 30;
    [SerializeField] private VisualEffect _visualEffect;
    
    void Start() {
        _aimLineRenderer.startColor =_defaultLazerColor;
        _aimLineRenderer.endColor =_defaultLazerColor;
    }
  
    private void FireProjectile(object sender, EventArgs e) {
        Projectile projectile = Instantiate(_prfProjectile, _firePoint.position, Quaternion.identity);
        projectile.transform.forward = _firePoint.forward;
        projectile.SetUpProjectile(_damage, _firePoint.forward*_projectileSpeed);
    }

    public override void StartClick() {
        FireProjectile(this, null);
        _aimLineRenderer.startColor =_fireLazerColor;
        _aimLineRenderer.endColor =_fireLazerColor;
        _visualEffect.Play();
    }

    public override void StopClick() {
        _aimLineRenderer.startColor = _defaultLazerColor;
        _aimLineRenderer.endColor = _defaultLazerColor;
    }
}