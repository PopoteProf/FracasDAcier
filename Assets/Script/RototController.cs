using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

public class RototController : MonoBehaviour, IDamagable
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private Camera _camera;
    [Header("LifeParameters")]
    //[SerializeField] private LocomotorBrainIK _locomotorBrainIK;
    //[SerializeField] private PhysicControllerFoot _physicController;
    //[SerializeField] private Rigidbody[] _rbRagdolls;
    //[SerializeField] private Collider[] _colliders;

    [Header("CannonParameters")]
    [SerializeField] private Transform _cannon;
    [SerializeField] private Weapon[] _weapons;

    private Weapon _currentWeapon;
    private int _currenWeaponID;
    private bool _isFire;

    private bool _isAlive = true;
    private bool _firstChange = true;
    RaycastHit hit;
    private InputAction _tabAction;

    private Coroutine _selectWeapon;

    public void Start() {
        foreach (var weapon in _weapons) {
            weapon.ChangeSelection(false);
        }
        StartCoroutine(SelectWeapon(0));
        _tabAction =InputSystem.actions.FindAction("Tab");
        _tabAction.started += ManageWeaponSwitch;
    }

    private void ManageWeaponSwitch(InputAction.CallbackContext obj) {
        Debug.Log("Switch gun");
        if (_selectWeapon != null) return;
        
        if (_currenWeaponID+1>=_weapons.Length) _selectWeapon = StartCoroutine(SelectWeapon(0));
        else _selectWeapon = StartCoroutine(SelectWeapon(_currenWeaponID + 1));
    }

    void Update() {
        //ManageMovement();
        if (!_isAlive) return;
        ManageAim();
        ManageFire();
        
    }

    private void ManageAim() {
        if (Physics.Raycast(_camera.ScreenPointToRay(Mouse.current.position.value),  out hit)) {
            _cannon.forward = hit.point - _cannon.position;
        }
        
    }

    private void ManageFire() {
        if (Mouse.current.leftButton.isPressed) {
            if (!_isFire) {
                _isFire = true;
                _currentWeapon.StartClick();
            }
        }
        else if (_isFire && !Mouse.current.rightButton.isPressed) {
            _isFire = false;
            _currentWeapon.StopClick();
        }
    }

    

    private IEnumerator SelectWeapon(int id)
    {
        // VFX
        List<VisualEffect> currentWeaponEffects =  new List<VisualEffect>();
        if (!_firstChange)
        {
            currentWeaponEffects = _currentWeapon.GetVisualEffects();
            for (int i = 0; i < currentWeaponEffects.Count; i++)
            {
                currentWeaponEffects[i].Play();
            }
        }
        
        yield return new WaitForSeconds(0.5f);
        if (_currentWeapon != null) {
            _currentWeapon.ChangeSelection(false);
        }
        _currentWeapon = _weapons[id];
        _currenWeaponID = id;
        _currentWeapon.ChangeSelection(true);
        
        // VFX
        List<VisualEffect> newWeaponeffects =  new List<VisualEffect>();
        if (!_firstChange)
        {
            newWeaponeffects = _currentWeapon.GetVisualEffects();
            // VFX
            for (int i = 0; i < newWeaponeffects.Count; i++)
            {
                newWeaponeffects[i].Play();
            }
        }
        
        yield return new WaitForSeconds(0.5f);
        // VFX
        for (int i = 0; i < currentWeaponEffects.Count; i++)
        {
            currentWeaponEffects[i].Stop();
        }
        for (int i = 0; i < newWeaponeffects.Count; i++)
        {
            newWeaponeffects[i].Stop();
        }
        
        _firstChange = false;
        _selectWeapon = null;
    }

   

    

    //[ContextMenu("TestRagdoll")]
    //private void TriggerRagdoll() {
    //    foreach (var rb in _rbRagdolls) {
    //        rb.isKinematic = false;
    //        rb.useGravity = true;
    //        rb.constraints = RigidbodyConstraints.None;
    //    }
//
    //    foreach (var col in _colliders) {
    //        col.enabled = true;
    //    }
//
    //    _locomotorBrainIK.enabled = false;
    //    _physicController.enabled = false;
    //    _isAlive = false;
    //}
    

    //private void ManageMovement()
    //{
    //    Vector2 moveInputVec =InputSystem.actions.FindAction("Move").ReadValue<Vector2>();
    //    Vector3 moveVec = new Vector3(0,0,moveInputVec.y);
    //    moveVec *= _moveSpeed;
    //    moveVec*=Time.deltaTime;
    //    transform.Translate(moveVec);
//
    //    float rotation = moveInputVec.x;
    //    rotation *= _rotateSpeed;
    //    rotation *= Time.deltaTime;
    //    transform.Rotate(transform.up, rotation);
    //}
    public void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
       Debug.Log("TakeDamage");
    }
}