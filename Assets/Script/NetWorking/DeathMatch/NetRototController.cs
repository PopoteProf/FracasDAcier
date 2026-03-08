using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class NetRototController : NetworkBehaviour
{
    [SerializeField] private bool _isActivePlayer;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private Camera _camera;
    [Header("CannonParameters")] [SerializeField] private Transform _cannon;
    
    [SerializeField] private NetWeapon[] _weapons;

    private NetWeapon _currentWeapon;
    private int _currenWeaponID;
    private bool _isFire;

    private bool _isAlive=true;
    RaycastHit hit;
    private InputAction _tabAction;

    public NetworkVariable<int> _weaponID = new NetworkVariable<int>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public void Start() {
        foreach (var weapon in _weapons) {
            weapon.ChangeSelection(false);
        }
        SelectWeapon(0);
        
        if (_currenWeaponID != _weaponID.Value) {
            SelectWeapon(_weaponID.Value);
        }
        if (!_isActivePlayer)return;
        _tabAction =InputSystem.actions.FindAction("Tab");
        _tabAction.started += ManageWeaponSwitch;
    }

    private void ManageWeaponSwitch(InputAction.CallbackContext obj) {
        Debug.Log("Switch gun");
        
        //if (_currenWeaponID+1>=_weapons.Length) SelectWeapon(0);
        //else SelectWeapon(_currenWeaponID+1);
        
        if (_currenWeaponID+1>=_weapons.Length) _weaponID.Value = 0;
        else _weaponID.Value = _currenWeaponID+1;
    }

    void Update() {
        if (!_isAlive||!_isActivePlayer) return;
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
    

    private void SelectWeapon(int id) {
        if (_currentWeapon != null) {
            _currentWeapon.ChangeSelection(false);
        }
        _currentWeapon = _weapons[id];
        _currenWeaponID = id;
        _currentWeapon.ChangeSelection(true);
    }

    public void SetUpElement() {
        _isActivePlayer = true;
        _camera = Camera.main;
    }

    public override void OnNetworkSpawn()
    {
        _weaponID.OnValueChanged+= OnWeaponValueChanged;
        SelectWeapon(_weaponID.Value);
        base.OnNetworkSpawn();
    }
    

    public override void OnNetworkDespawn()
    {
        _weaponID.OnValueChanged -= OnWeaponValueChanged;
        base.OnNetworkDespawn();
    } 
    
    private void OnWeaponValueChanged(int previousValue, int newValue) {
       SelectWeapon(newValue); 
    }
    
    
}