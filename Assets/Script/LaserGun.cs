using System;
using UnityEngine;

public class LaserGun : Weapon
{
    [SerializeField] private bool _usPreFireDelay;
    [SerializeField] private float _preFireDelay = 0.5f;
    [SerializeField] private bool _usConstantDamageTick;
    [SerializeField] private float _damgeTickPerSecond =4;
    [SerializeField] private bool _usSpawnImpactOnDamageTick;
    [SerializeField, ColorUsage(true, true)] private Color _defaultLazerColor;
    [SerializeField, ColorUsage(true, true)] private Color _fireLazerColor;
    [SerializeField] private GameObject _prfFireImpact;
    [SerializeField] private int _damage =1;
    
    private bool _isFire;
    private PopoteTimer _preFireTimer;
    private PopoteTimer _damageTickTimer;

    private void Awake()
    {
        _preFireTimer = new PopoteTimer(_preFireDelay);
        _preFireTimer.OnTimerEnd+= OnPreFireTimerEnd;
        _damageTickTimer = new PopoteTimer(1/_damgeTickPerSecond);
        _damageTickTimer.OnTimerEnd += OnDamageTickTimerEnd;
    }

    private void OnDamageTickTimerEnd(object sender, EventArgs e) {
        DoDamage();
    }

    private void OnPreFireTimerEnd(object sender, EventArgs e) {
        DoFire();
    }

    void Start() {
        _aimLineRenderer.startColor =_defaultLazerColor;
        _aimLineRenderer.endColor =_defaultLazerColor;
    }

    protected override void Update()
    {
        base.Update();
        _preFireTimer.UpdateTimer();
        _damageTickTimer.UpdateTimer();
    }

    
    
    public override void StartClick() {
        if (_usPreFireDelay)
        {
            _preFireTimer.Play();
        }
        else
        {
            DoFire();
        }
    }

    private void DoFire()
    {
        if (_prfMuzzleFire != null) Instantiate(_prfMuzzleFire, _firePoint.position,_firePoint.rotation);
        if( _fireImpulseSource!=null)_fireImpulseSource.GenerateImpulse();
        if( _prfFireImpact){ 
            GameObject go = Instantiate(_prfFireImpact, hit.point, Quaternion.identity);
            go.transform.up = hit.normal;
        }

        if (hit.collider.GetComponent<IDamagable>() != null) {
            IDamagable target = hit.collider.GetComponent<IDamagable>();
            target.TakeDamage(_damage, hit.point, hit.normal);
        }
        _aimLineRenderer.startColor =_fireLazerColor;
        _aimLineRenderer.endColor =_fireLazerColor;
        if(_usConstantDamageTick){ _damageTickTimer.Play();}
    }

    private void DoDamage()
    {
        if (hit.collider.GetComponent<IDamagable>() != null) {
            IDamagable target = hit.collider.GetComponent<IDamagable>();
            target.TakeDamage(_damage, hit.point, hit.normal);
        }
        if (_usSpawnImpactOnDamageTick && _prfFireImpact){ 
            GameObject go = Instantiate(_prfFireImpact, hit.point, Quaternion.identity);
            go.transform.up = hit.normal;
        }
        _damageTickTimer.Play();
    }
    

    public override void StopClick()
    {
        _aimLineRenderer.startColor = _defaultLazerColor;
        _aimLineRenderer.endColor = _defaultLazerColor;
        _preFireTimer.Pause();
        _damageTickTimer.Pause();
    }
}