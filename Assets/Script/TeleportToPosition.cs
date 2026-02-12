using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class TeleportToPosition : MonoBehaviour
{
    public float Speed;
    public float SmoothTime;
    public float MinDistanceBeforeMove;
    [SerializeField] private bool _move;
    [SerializeField] private Vector3 _destination;
    [SerializeField] private Vector3 _startPosition;
    [SerializeField] private AnimationCurve _curve;
    
    [SerializeField] private float _distance;
    Vector3 _velocity;
    private Vector3 _lerpPosition;

    private void Awake()
    {
        _destination = transform.position;
    }

    private void OnEnable()
    {
        EventBus.OnPlayerClickedOnGround += TakeDistance;
    }

    private void OnDisable()
    {
        EventBus.OnPlayerClickedOnGround -= TakeDistance;
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 3 , Color.blue, 0.1f);
        Vector3 LookVector = _destination - transform.position;
        Vector3 startRayCast = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        Debug.DrawRay(transform.position, LookVector , Color.yellow);
        
        if (_move)
        {
            Vector3 currentPosition = new Vector3(transform.position.x, 0, transform.position.z);
            float currentDistance = Vector3.Distance(currentPosition, _destination);
            float normalizedDistance = currentDistance / _distance;
            
            _lerpPosition = Vector3.SmoothDamp(_lerpPosition, _destination, ref _velocity , SmoothTime * Time.deltaTime,  Speed);
            transform.forward = Vector3.RotateTowards(transform.forward, LookVector,10 * Mathf.Deg2Rad, 500);
            
            Vector3 position = _lerpPosition;
            position.y = _curve.Evaluate(normalizedDistance) + _lerpPosition.y;
            transform.position = position;
            
            Debug.Log(position);
            //Debug.Log(_curve.Evaluate(normalizedDistance));
            if (Vector3.Distance(transform.position, _destination) < 0.2f)
            {
                _move = false;
            }
        }
        
        RaycastHit hit;
        if (Physics.Raycast(startRayCast, Vector3.down * 100, out hit) && !_move)
        {
            Vector3 position = transform.position;
            position.y = hit.point.y + 0.5f;
            transform.position = position;
        }
    }

    public void TakeDistance(Vector3 point)
    {
        if (Vector3.Distance(transform.position, point) < MinDistanceBeforeMove) return;
        _move = true;
        _lerpPosition = transform.position;
        _destination = point;
        _startPosition = new Vector3(transform.position.x,0, transform.position.z);
        _destination = new Vector3(_destination.x, 0, _destination.z);
        _distance = Vector3.Distance(_startPosition, _destination);
       
    }
}
