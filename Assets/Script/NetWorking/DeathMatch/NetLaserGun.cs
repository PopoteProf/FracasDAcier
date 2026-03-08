using System;
using NUnit.Framework.Constraints;
using Unity.Netcode;
using UnityEngine;

public class NetLaserGun : NetWeapon
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

    private void Awake() {
        _preFireTimer = new PopoteTimer(_preFireDelay);
        _preFireTimer.OnTimerEnd+= OnPreFireTimerEnd;
        _damageTickTimer = new PopoteTimer(1/_damgeTickPerSecond);
        _damageTickTimer.OnTimerEnd += OnDamageTickTimerEnd;
    }

    private void OnDamageTickTimerEnd(object sender, EventArgs e) {
        DoDamageRpc();
    }

    private void OnPreFireTimerEnd(object sender, EventArgs e) {
        DoFireRpc(); }

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
            DoFireRpc();
        }
    }

    
    [Rpc(SendTo.Server)]
    private void DoFireRpc(
        RpcDelivery rpcDelivery  = RpcDelivery.Reliable,
        RpcInvokePermission rpcInvokePermission = RpcInvokePermission.Everyone,
        LocalDeferMode localDeferMode = LocalDeferMode.Default
        )
    {
        if( !IsServer&&!IsHost)return;
        if (_prfMuzzleFire != null) Instantiate(_prfMuzzleFire, _firePoint.position,_firePoint.rotation);
        if( _fireImpulseSource!=null)_fireImpulseSource.GenerateImpulse();
        PlayHitVFXRpc(hit.point, hit.normal);
        //if( _prfFireImpact){ 
        //    GameObject go = Instantiate(_prfFireImpact, hit.point, Quaternion.identity);
        //    go.transform.up = hit.normal;
        //}
        if (hit.collider == null) return;
        if (hit.collider.GetComponentInParent<IDamagable>() != null) {
            IDamagable target = hit.collider.GetComponentInParent<IDamagable>();
            target.TakeDamage(_damage, hit.point, hit.normal);
        }
        _aimLineRenderer.startColor =_fireLazerColor;
        _aimLineRenderer.endColor =_fireLazerColor;
        if(_usConstantDamageTick){ _damageTickTimer.Play();}
    }

    [Rpc(SendTo.Server)]
    private void DoDamageRpc()
    {
        if (hit.collider.GetComponentInParent<IDamagable>() != null) {
            IDamagable target = hit.collider.GetComponentInParent<IDamagable>();
            target.TakeDamage(_damage, hit.point, hit.normal);
        }
        if (_usSpawnImpactOnDamageTick && _prfFireImpact){ 
            GameObject go = Instantiate(_prfFireImpact, hit.point, Quaternion.identity);
            go.transform.up = hit.normal;
        }
        _damageTickTimer.Play();
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void PlayHitVFXRpc(Vector3 pos, Vector3 normal) {
        if( _prfFireImpact){ 
            GameObject go = Instantiate(_prfFireImpact, pos, Quaternion.identity);
            go.transform.up = normal;
        }
    }
    

    public override void StopClick()
    {
        _aimLineRenderer.startColor = _defaultLazerColor;
        _aimLineRenderer.endColor = _defaultLazerColor;
        _preFireTimer.Pause();
        _damageTickTimer.Pause();
    }
}

