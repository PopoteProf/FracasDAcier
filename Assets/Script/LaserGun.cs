using System;
using UnityEngine;
using UnityEngine.VFX;

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
    
    //VFX
    [SerializeField] private Transform _muzzlePivot;
    private GameObject _muzzleInstance;
    private VisualEffect _muzzleVFX;
    private static readonly int IsFiringID = Shader.PropertyToID("IsFiring");


    private void Awake()
    {
        _preFireTimer = new PopoteTimer(_preFireDelay);
        _preFireTimer.OnTimerEnd+= OnPreFireTimerEnd;
        _damageTickTimer = new PopoteTimer(1/_damgeTickPerSecond);
        _damageTickTimer.OnTimerEnd += OnDamageTickTimerEnd;
        
        SetupMuzzleVFX();
    }

    private void SetupMuzzleVFX()
    {
        if (_prfMuzzleFire != null)
        {
            _muzzleInstance = Instantiate(_prfMuzzleFire, _muzzlePivot.position, _muzzlePivot.rotation, _muzzlePivot);
            _muzzleVFX = _muzzleInstance.GetComponent<VisualEffect>();
            if (_muzzleVFX != null)
            {
                _muzzleVFX.SetBool(IsFiringID, false);
            }
        }
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
        StartMuzzleVFX();
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
        StopMuzzleVFX();
    }
    
    private void StartMuzzleVFX()
    {
        if (_muzzleVFX != null)
            _muzzleVFX.SetBool(IsFiringID, true);
    }
    private void StopMuzzleVFX()
    {
        if (_muzzleVFX != null)
            _muzzleVFX.SetBool(IsFiringID, false);
    }
}