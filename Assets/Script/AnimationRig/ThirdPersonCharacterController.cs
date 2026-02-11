using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonCharacterController : MonoBehaviour
{
    [SerializeField] private PlayerInteractor _playerInteractor;
    [SerializeField] private bool _freezControl;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _acceleration =1;
    [SerializeField] private float RotaitionSpeed =90;
    [SerializeField] private MultiAimConstraint _multiAimConstraint;
    
    
    
    CharacterController _characterController;
    float _moveSpeed;
    LookUpTrigger _lookUpTrigger;
    private Transform _buttonTransform;
    
    InputAction _moveAction;
    InputAction _fireAction;
    
    public float LookSpeed;
    public AnimationCurve LookCurve;
    public Transform LookTarget;
    
    void Start() {
        _characterController =  GetComponent<CharacterController>();
        _moveAction = InputSystem.actions.FindAction("Move");
        
        _fireAction = InputSystem.actions.FindAction("Attack");
        _fireAction.started += FireActionOnstarted;
        _multiAimConstraint.weight = 0;
    }

    

    private void Update() {
        ManageMouvement();
        LookingTarget();
    }

    private void ManageMouvement() {
        Vector2 inputVec =Vector2.zero;
        if( _freezControl)inputVec = _moveAction.ReadValue<Vector2>();
        
        if (inputVec.y > 0.5f) {
            _moveSpeed += _acceleration * Time.deltaTime;
        }
        else if (inputVec.y < -0.5f) {
            _moveSpeed -= _acceleration * Time.deltaTime;
        }
        else {
            if (_moveSpeed != 0) {
                if (_moveSpeed> 0) {
                    _moveSpeed= Mathf.Clamp(_moveSpeed- _acceleration *Time.deltaTime,0,1);
                }
                if (_moveSpeed < 0) {
                    _moveSpeed= Mathf.Clamp(_moveSpeed+ _acceleration *Time.deltaTime,-1,0);
                }
            }
        }
        _moveSpeed = Mathf.Clamp(_moveSpeed, -1, 1);
        
        Vector3 moveVec =  _moveSpeed *moveSpeed*Time.deltaTime*transform.forward ;
        _characterController.Move(moveVec);
        if( _moveSpeed<0)_animator.SetFloat("Speed", -_characterController.velocity.magnitude);
        else _animator.SetFloat("Speed", _characterController.velocity.magnitude);

        if (inputVec.x > 0.5f)
        {
            transform.Rotate(transform.up, RotaitionSpeed*Time.deltaTime);
            _animator.SetFloat("Rotation", 1);
        }
        else if (inputVec.x < -0.5f)
        {
            transform.Rotate(transform.up, -RotaitionSpeed*Time.deltaTime);
            _animator.SetFloat("Rotation", -1);
        }
        else
        {
            _animator.SetFloat("Rotation", 0); 
        }
    }

    public void SetUpLookUpTrigger(LookUpTrigger trigger)
    {
        _lookUpTrigger = trigger;
        LookTarget.transform.position = trigger.transform.position;
    }

    public void LeaveLookUpTrigger(LookUpTrigger trigger)
    {
        if (trigger == _lookUpTrigger) 
        {
            _lookUpTrigger = null;
        }
    }

    private void LookingTarget()
    {
        Vector3 LookVector = LookTarget.position - transform.position;
        LookVector.y = 0;
        Debug.Log(Vector3.Dot(LookVector.normalized, transform.forward));
        Debug.DrawRay(transform.position, LookVector, Color.aquamarine);
        Debug.DrawRay(transform.position, transform.forward * 2, Color.blue);
        
        LookTarget.transform.position = _lookUpTrigger.transform.position;
        
        if (Vector3.Dot(LookVector.normalized, transform.forward) < 0.5f || _lookUpTrigger == null )
        {
            LookForward();
            Debug.LogError("Return");
            return;
        }
        _multiAimConstraint.weight += LookSpeed * Time.deltaTime; 
    }

    private void LookForward()
    {
        _multiAimConstraint.weight -= LookSpeed * Time.deltaTime; 
        Debug.LogWarning("LookForward");
    }
    
    private void FireActionOnstarted(InputAction.CallbackContext obj) 
    {
        Debug.Log("FireActionOnstarted");
        if (_playerInteractor!=null)_playerInteractor.Interact(this);
    }
    
    
}