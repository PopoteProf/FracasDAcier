using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.Serialization;
using Vector3 = UnityEngine.Vector3;

public class FollowScript : MonoBehaviour
{
    [Header("Target Ref")]
    [SerializeField] private Transform _target;
    
    [FormerlySerializedAs("_maxDistanceMove")]
    [Header("Move System Settings")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _smoothTime;
    [SerializeField] private float _minDistanceMove;
    [SerializeField] private float _jumpForce;
    [SerializeField] private Vector3 _currentVelocity;
    [SerializeField] private AnimationCurve _offsetYCurve;
    
    [Header("Rotate System Settings")]
    [SerializeField] private float _maxRadians;
    [SerializeField] private float _maxMagnitude;

    [Header("Debug Settings")]
    [SerializeField] private bool _isUseMoveToward;
    [SerializeField] private bool _isUseRotateToward;
    [SerializeField] private bool _isUseMouseTarget;

    private bool _isMoving;
    private Vector3 _targetPosition;
    private Vector3 _lastPosition;
    private Vector3 _pos;
    private Vector3 _dir;

    private void Start()
    {
        if (_target == null)
        {
            _isUseMouseTarget = true;
        }
        else
        {
            _isUseMouseTarget = false;
        }
    }

    private void Update()
    {
        //Verification min distance
        if (IsCheckDistance())
        {
            //Moving System 
            ManageMouvement();
        }

        if (Vector3.Distance(transform.position, _targetPosition) <= 0.2f)
        {
            _isMoving = false;
        }
        
        transform.forward = _dir;
        transform.position = _pos;
    }

    private bool IsCheckDistance()
    {
        if (Vector3.Distance(RaycastController._lastHitPoint, transform.position) > _minDistanceMove && !_isMoving)
        {
            _targetPosition =  RaycastController._lastHitPoint;
            _lastPosition = transform.position;
            _isMoving = true;
        }
        return _isMoving;
    }

    private void ManageMouvement()
    {
        if (!_isUseMouseTarget)
        {
            _targetPosition = _target.position;
        }
        _pos = transform.position;
        _dir = transform.forward;
        
        if (_isUseMoveToward)
        {
            _pos = Vector3.MoveTowards(transform.position, _targetPosition, _moveSpeed * Time.deltaTime);
        }
        else
        {
            _pos = Vector3.SmoothDamp(transform.position, _targetPosition, ref _currentVelocity, _smoothTime ,_moveSpeed,Time.deltaTime);
        }

        RaycastHit hit;
        Vector3 origin = transform.position + Vector3.up * 5;
        Vector3 direction = Vector3.down;

        if (Physics.Raycast(origin, direction, out hit, 100f))
        {
            _pos.y = hit.point.y;
        }
        
        float totalDistance = Vector3.Distance(_lastPosition, _targetPosition);
        float currentDistance = Vector3.Distance(_lastPosition, _pos);
        
        if (totalDistance > 0.001f)
        {
            float progress = Mathf.Clamp01(currentDistance / totalDistance);
            float lerpPos = Mathf.Lerp(_lastPosition.y, _targetPosition.y, progress);
            float arc = _offsetYCurve.Evaluate(progress) * _jumpForce;

            lerpPos += arc;

            _pos.y = lerpPos;
        }
            
        if (_isUseRotateToward)
        {
            Vector3 rotate = (RaycastController._lastHitPoint - transform.position).normalized;
            _dir = Vector3.RotateTowards(transform.forward,rotate,_maxRadians * Mathf.Deg2Rad,_maxMagnitude );
        }
    }
}
