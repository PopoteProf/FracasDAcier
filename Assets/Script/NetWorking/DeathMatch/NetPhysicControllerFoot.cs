using System;
using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class NetPhysicControllerFoot : NetworkBehaviour , IDamagable
{
    public event EventHandler OnMeckDestroyed;
    
    [SerializeField] private bool _isActivePlayer;
    [SerializeField] private float _raytoGroudLength = 2;
    [SerializeField] private float _distanceToGround = 1.5f;
    [SerializeField] private LayerMask _groundMask ;
    [SerializeField] private float _upWardPower = 100;
    [SerializeField] private float _springPower = 2;
    [Space(10)]
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _rotationPower = 100f;
    [Space(10)] 
    [SerializeField] private LegLocomotorIK _leftFoot;
    [SerializeField] private LegLocomotorIK _rightFoot;

    [Space(10), Header("Heath")] 
    [SerializeField] private int _maxHealth = 10;
    [SerializeField] private NetworkVariable<int> _currentHeath = new NetworkVariable<int>(default,NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);


    [Space(10), Header("Spawn Setup")] 
    [SerializeField] private CinemachineCamera _prfVirtuelCamere;
    [SerializeField] private NetRototController _netRototController;
    
    private Rigidbody _rb;
    private InputAction _moveAction;

    private void Awake() {
        _rb = GetComponent<Rigidbody>();
        
    }

    void Start() {
        if( _isActivePlayer) _moveAction = InputSystem.actions.FindAction("Move");
    }

    void FixedUpdate() {
        //if (Physics.Raycast(new Ray(transform.position, Vector3.down), out RaycastHit hit, _raytoGroudLength,
        //        _groundMask)) {
            
            //float footMidPos = Mathf.Abs((_leftFoot.MidPos.y+_rightFoot.MidPos.y)/2- transform.position.y);
            //Debug.Log("Foot hight:"+ footMidPos);
            //Debug.DrawRay(transform.position, hit.point, Color.blueViolet);
            //float springMod = _distanceToGround - footMidPos / _distanceToGround;
            ////Debug.Log("spring mode = " + springMod);
            //_rb.AddForce(Vector3.up * _upWardPower*Time.fixedDeltaTime*(_upWardPower*_springPower*springMod));
        //}
        float footLastPos = Mathf.Abs((_leftFoot.MidPos.y+_rightFoot.MidPos.y)/2)+_distanceToGround;
        //transform.position = new Vector3(transform.position.x,_distanceToGround+footLastPos,transform.position.z);
        float springMod = (footLastPos-transform.position.y)/_distanceToGround;
        _rb.AddForce(Vector3.up * _upWardPower*Time.fixedDeltaTime*(_upWardPower*_springPower*springMod));
        if (!_isActivePlayer) return;
        Vector2 moveInput = _moveAction.ReadValue<Vector2>();
        _rb.AddForce(transform.forward*moveInput.y*_moveSpeed*Time.fixedDeltaTime);
        _rb.AddTorque(transform.up*moveInput.x*_rotationPower*Time.fixedDeltaTime);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position,transform.position+Vector3.down*_raytoGroudLength );
    }

   
    public void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitNormal) {
        _currentHeath.Value = Mathf.FloorToInt(_currentHeath.Value - damage);
        PlayTakeDamageRpc();
        if(  _currentHeath.Value <= 0) ManageDeath();
        Debug.Log("Meck take Damage   = "+ damage);
    }

    private void ManageDeath() {
        PlayDeathRpc();
        GetComponent<NetworkObject>().Despawn();
    }
    
    [Rpc(SendTo.ClientsAndHost)]
    private void PlayTakeDamageRpc() {
        Debug.Log("PlayTakeDamage", this);
    }
    [Rpc(SendTo.ClientsAndHost)]
    private void PlayDeathRpc() {
        Debug.Log(" Meck die", this);
        OnMeckDestroyed?.Invoke(this, EventArgs.Empty);
    }

    [ContextMenu("TestCurrentHP")]
    private void TestCurrentHP()
    {
        Debug.Log(_currentHeath.Value);
    }

    public override void OnNetworkSpawn() {
        Debug.Log("MeckSpawn with de owerd id ="+ OwnerClientId);

        if (IsServer)_currentHeath.Value = _maxHealth;
        _isActivePlayer = IsOwner;
        if( _isActivePlayer ) {SetUpElement();}
        base.OnNetworkSpawn();
    }

    private void SetUpElement() {
        CinemachineCamera Vcam = Instantiate(_prfVirtuelCamere, transform.position, transform.rotation);
        Vcam.Follow = transform;
        Vcam.LookAt = transform;
        _netRototController.SetUpElement();
    }
}