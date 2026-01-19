using System;
using UnityEngine;

public class MiniGun : Weapon
{
    [SerializeField, ColorUsage(true, true)] private Color _defaultLazerColor;
    [SerializeField, ColorUsage(true, true)] private Color _fireLazerColor;
    [SerializeField] private int _damage =2;
    [SerializeField] private Projectile _prfProjectile;
    [SerializeField] private float _projectileSpeed = 50;
    [SerializeField] private float _fireRate =3;
    
    private PopoteTimer _fireTimer;
    
    void Awake() {
        _aimLineRenderer.startColor =_defaultLazerColor;
        _aimLineRenderer.endColor =_defaultLazerColor;
        _fireTimer = new PopoteTimer(1f/_fireRate);
        _fireTimer.OnTimerEnd += FireProjectile;
    }
    protected override void Update() {
        _fireTimer.UpdateTimer();
        base.Update();
        
    }

    private void FireProjectile(object sender, EventArgs e) {
        Projectile projectile = Instantiate(_prfProjectile, _firePoint.position, Quaternion.identity);
        projectile.transform.forward = _firePoint.forward;
        projectile.SetUpProjectile(_damage, _firePoint.forward*_projectileSpeed);
        _fireTimer.Play();
    }

    public override void StartClick() {
        _fireTimer.Play();
        FireProjectile(this, null);
        _aimLineRenderer.startColor =_fireLazerColor;
        _aimLineRenderer.endColor =_fireLazerColor;
    }

    public override void StopClick() {
         _fireTimer.Pause();
        _aimLineRenderer.startColor = _defaultLazerColor;
        _aimLineRenderer.endColor = _defaultLazerColor;
    }
}