using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class ControlCube : MonoBehaviour {
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _maxMoveSpeed;
    private Rigidbody _rb;
    private InputAction _move;

    private void Awake() {
        _rb = GetComponent<Rigidbody>();
    }
    private void Start() {
        _move = InputSystem.actions["Move"];
        
    }

    // Update is called once per frame
    void Update() {
        ManageMovement();
    }

    private void ManageMovement() {
        Vector2 rawValue = _move.ReadValue<Vector2>();
        Vector3 movement = new Vector3(rawValue.x, 0, rawValue.y); 
        _rb.AddForce(movement * _moveSpeed * Time.deltaTime);
        if (_rb.linearVelocity.magnitude > _maxMoveSpeed) {
            _rb.linearVelocity = _rb.linearVelocity.normalized * _maxMoveSpeed;
        }
        Vector3  dir =  _rb.linearVelocity;
        if (dir.magnitude > 0.1f) {
            dir.y = 0;
            transform.forward = dir;
        }
    }
}