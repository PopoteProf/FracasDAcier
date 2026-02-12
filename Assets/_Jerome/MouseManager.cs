using UnityEngine;
using UnityEngine.InputSystem;

public class MouseManager : MonoBehaviour
{
    [SerializeField] private LayerMask _clickLayer;
    [SerializeField] private Camera _cam;
    [SerializeField] private GameObject _marker; // Visual indicator
    [SerializeField] private GameObject _capsule;
    [SerializeField] private float _maxDistance;
    
    [Header("SmoothDamp")]
    [SerializeField] private float _smoothTime;
    [SerializeField] private float _maxSpeed;
    
    [Header("Jump")]
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _jumpDistance; // Distance from target to trigger jump
    [SerializeField] private float _jumpDuration = 1f; // How long the jump takes
    
    [Header("Movement Mode")]
    [SerializeField] private bool _useJumpInsteadOfMove = true; // Toggle between jump and move
    
    private Vector3 _velocity;
    private Vector3 _targetPosition;
    private bool _isMoving;
    private bool _isJumping;
    
    // Jump variables
    private Vector3 _jumpStart;
    private Vector3 _jumpEnd;
    private Vector3 _jumpPeak;
    private float _jumpTime;

    public Vector3 MovePosition;

    private void Update()
    {
        Vector3? worldPos = GetMouseWorldPosition();

        if (!worldPos.HasValue) return;
        
        if (_marker) _marker.transform.position = worldPos.Value;

        if (!_capsule) return;
        
        MovePosition = worldPos.Value;
        
        // Check if we should start moving/jumping to a new position
        float distanceToMouse = Vector3.Distance(_capsule.transform.position, worldPos.Value);
        
        if (distanceToMouse > _maxDistance && !_isMoving && !_isJumping)
        {
            // Set new target position and start movement
            _targetPosition = worldPos.Value;
            // StartMoveToPosition(_targetPosition);
            StartJumpToPosition(_targetPosition);
            
        }
        
        // PerformMove();
        if (_isJumping)PerformJump();
        
    }

    #region Movement Methods
    
    private void StartMoveToPosition(Vector3 target)
    {
        _targetPosition = target;
        _isMoving = true;
        _isJumping = false;
        _velocity = Vector3.zero;
    }
    
    private void PerformMove()
    {
        // Move towards target with SmoothDamp
        _capsule.transform.position = Vector3.SmoothDamp(
            _capsule.transform.position,
            _targetPosition,
            ref _velocity,
            _smoothTime,
            _maxSpeed,
            Time.deltaTime
        );

        // Check if we've reached the target
        float distanceToTarget = Vector3.Distance(_capsule.transform.position, _targetPosition);
        if (distanceToTarget <= 0.1f)
        {
            StopMove();
        }
    }
    
    private void StopMove()
    {
        _isMoving = false;
        _velocity = Vector3.zero;
    }
    
    #endregion

    #region Jump Methods
    
    private void StartJumpToPosition(Vector3 target)
    {
        _isJumping = true;
        
        // Set up jump arc
        _jumpStart = _capsule.transform.position;
        _jumpEnd = target;
        
        // Calculate peak point (halfway horizontally, plus height)
        Vector3 halfway = Vector3.Lerp(_jumpStart, _jumpEnd, 0.5f);
        _jumpPeak = halfway;
        _jumpPeak.y += _jumpHeight;
        
        _jumpTime = 0f;
    }
    
    private void PerformJump()
    {
        _jumpTime += Time.deltaTime * _jumpDuration;
        
        if (_jumpTime <= 1f)
        {
            // Quadratic Bezier curve for parabolic arc
            // ac = lerp(start, peak, t)
            // cb = lerp(peak, end, t)
            // X = lerp(ac, cb, t)
            
            Vector3 ac = Vector3.Lerp(_jumpStart, _jumpPeak, _jumpTime);
            Vector3 cb = Vector3.Lerp(_jumpPeak, _jumpEnd, _jumpTime);
            _capsule.transform.position = Vector3.Lerp(ac, cb, _jumpTime);
        }
        else
        {
            StopJump();
        }
    }
    
    private void StopJump()
    {
        _capsule.transform.position = _jumpEnd;
        _isJumping = false;
        _jumpTime = 0f;
    }
    
    #endregion



    private Vector3? GetMouseWorldPosition()
    {
        Ray ray = _cam.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _clickLayer))
        {
            return hit.point;
        }
        
        return null;
    }
}