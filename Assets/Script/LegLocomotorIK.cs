using System;
using Unity.Cinemachine;
using UnityEngine;

public class LegLocomotorIK : MonoBehaviour {
    
    public event EventHandler OnLegStartMouvement;
    public event EventHandler OnLegEndMouvement;
    [SerializeField] private float _legStepSpeed;
    [SerializeField] private float _legHeightApex;
    [SerializeField] private AnimationCurve _legHeightCurve;
    [Space(10)]
    [SerializeField] private float _legMoveThreshold;
    [Header("RayCasterParameters")]
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _raycasterAmplitudeMode;
    [SerializeField] private Transform _rayCaster;
    [SerializeField] private float _raycasterSpringSize =1;
    [SerializeField] private float _raycasterPringSpeed =1;
    [SerializeField] private AnimationCurve _raycasterSpringMod = AnimationCurve.EaseInOut(0,0,1,1);
    [Space(5)]
    [SerializeField] private Transform _footTarget;

    [SerializeField] private CinemachineImpulseSource _impulseSource;
    [SerializeField] private ParticleSystem _particleSystemFootStep;

    [SerializeField] private bool _DrawDebug;

    private bool _legMoveLock;
    private Vector3 _velocity;
    private Vector3 _hit;
    private Vector3 _newPos;
    private Vector3 _lastPos;
    private Vector3 _lastVelocityPos; 
    private float _timer;
    private bool _isMoving;

    public bool LegMoveLock {
        get { return _legMoveLock; }
        set { _legMoveLock = value; }
    }

    public Vector3 LastPos {
        get => _lastPos;
        set => _lastPos = value;
    }

    public Vector3 MidPos {
        get => Vector3.Lerp(_lastPos, _newPos, _timer / _legStepSpeed);
    }

    public void SetCurrentVelocity(Vector3 velocity) {
       // _velocity = velocity;
    }

    private void Start() {
        _lastVelocityPos = transform.position;
    }

    public void Update() {
        CalculateVelocity();
        RayCasterPos();
        if (_isMoving) {
            ManagerLegMovement();
            return;
        }
        if(_legMoveLock)return;
        
        //_rayCaster.position = transform.position +_velocity*_raycasterAmplitudeMode;
        RaycastHit hit;
        if (Physics.Raycast(_rayCaster.position, _rayCaster.forward, out hit, Mathf.Infinity, _groundLayer)) {
            if (Vector3.Distance(_lastPos, hit.point) > _legMoveThreshold) {
                StartLegMovement(hit.point);
                _hit = hit.point;
            }
        }
    }

    public void StartLegMovement(Vector3 newPos) {
        _isMoving = true;
        _timer = 0;
        _newPos = newPos;
        OnLegStartMouvement?.Invoke(this, EventArgs.Empty);
    }

    private void ManagerLegMovement() {
        _timer += Time.deltaTime;
        float t = _timer / _legStepSpeed;
        Vector3 feetPos = Vector3.Lerp(_lastPos, _newPos, t);
        feetPos.y = (Mathf.Lerp(_lastPos.y, _newPos.y, _legHeightCurve.Evaluate(t) ))+_legHeightApex*_legHeightCurve.Evaluate(t);
        _footTarget.position = feetPos;
        if (_timer >= _legStepSpeed) {
            EndLegMovement();
        }
    }

    private void EndLegMovement() {
        _isMoving = false;
        _lastPos = _newPos;
        _footTarget.position = _newPos;
        OnLegEndMouvement?.Invoke(this, EventArgs.Empty);
        if( _impulseSource)_impulseSource.GenerateImpulse();
        if (_particleSystemFootStep) _particleSystemFootStep.Play();
    }

    private void RayCasterPos() {
        Vector3 targetPos = transform.position + _velocity * _raycasterAmplitudeMode;
        float t = Vector3.Distance(targetPos, _rayCaster.position) / _raycasterSpringSize;
        float moveLength = _raycasterPringSpeed * _raycasterSpringMod.Evaluate(t)*Time.deltaTime;
        
        Vector3 additiveVec =   (targetPos-_rayCaster.position).normalized*moveLength;
        _rayCaster.position = _rayCaster.position += additiveVec;
    }

    private void CalculateVelocity() {
        _velocity = (transform.position - _lastVelocityPos)/Time.deltaTime;
        _lastVelocityPos = transform.position;
    }

    private void OnDrawGizmos() {
        if (!_DrawDebug) return;
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, _rayCaster.transform.position);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_rayCaster.position, _hit);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(_hit, _newPos);
        Gizmos.DrawLine(_hit, _lastPos);
    }
}