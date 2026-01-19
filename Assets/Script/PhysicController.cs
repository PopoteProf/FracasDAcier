using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PhysicController : MonoBehaviour
{
    [SerializeField] private float _raytoGroudLength = 2;
    [SerializeField] private float _distanceToGround = 1.5f;
    [SerializeField] private LayerMask _groundMask ;
    [SerializeField] private float _upWardPower = 100;
    [SerializeField] private float _springPower = 2;
    [Space(10)]
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _rotationPower = 100f;
    
    private Rigidbody _rb;
    private InputAction _moveAction;

    private void Awake() {
        _rb = GetComponent<Rigidbody>();
    }

    void Start() {
        _moveAction = InputSystem.actions.FindAction("Move");
    }

    void FixedUpdate() {
        if (Physics.Raycast(new Ray(transform.position, Vector3.down), out RaycastHit hit, _raytoGroudLength,
                _groundMask)) {
            float springMod = _distanceToGround - hit.distance / _distanceToGround;
            Debug.Log("spring mode = " + springMod);
            _rb.AddForce(Vector3.up * _upWardPower*Time.fixedDeltaTime*(_upWardPower*_springPower*springMod));
        }
        Vector2 moveInput = _moveAction.ReadValue<Vector2>();
        _rb.AddForce(transform.forward*moveInput.y*_moveSpeed*Time.fixedDeltaTime);
        _rb.AddTorque(transform.up*moveInput.x*_rotationPower*Time.fixedDeltaTime);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position,transform.position+Vector3.down*_raytoGroudLength );
    }
}