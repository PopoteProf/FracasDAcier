using Unity.VisualScripting;
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

    [SerializeField] MultiAimConstraint  _multiAimConstraint;
    [SerializeField] Transform _target;
    [SerializeField] AnimationCurve _moveCurve;
    [SerializeField] private Transform _spine;
    
    
    CharacterController _characterController;
    float _moveSpeed;
    LookUpTrigger _lookUpTrigger;
    private Transform _buttonTransform;
    [SerializeField]private float _timer;
    
    InputAction _moveAction;
    InputAction _fireAction;
    
    void Start() {
        _characterController =  GetComponent<CharacterController>();
        _moveAction = InputSystem.actions.FindAction("Move");
        
        _fireAction = InputSystem.actions.FindAction("Attack");
        _fireAction.started += FireActionOnstarted;

    }

    

    private void Update() {
        ManageMouvement();
        
        if (_lookUpTrigger != null)
        {
            Vector3 origin = _target.position -  _spine.position;
            float dot = Vector3.Dot( _spine.forward.normalized,origin.normalized);
            Debug.Log(dot);
            if (_multiAimConstraint.weight < 1 && dot >= 0.1)
            {
                _timer += Time.deltaTime;
                _multiAimConstraint.weight = Mathf.Lerp(0, 1,_moveCurve.Evaluate(_timer) );
            }
            else
            {
                if (_multiAimConstraint.weight > 0)
                {
                    _timer -= Time.deltaTime;
                    _multiAimConstraint.weight = Mathf.Lerp(0, 1, _moveCurve.Evaluate(_timer));
                }
            }
            _target.position = _lookUpTrigger.LookUpTarget.position;
        }

        if (_lookUpTrigger == null && _multiAimConstraint.weight > 0)
        {
            _timer -= Time.deltaTime;
            _multiAimConstraint.weight = Mathf.Lerp(0, 1, _moveCurve.Evaluate(_timer));
        }
        
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

    public void SetUpLookUpTrigger(LookUpTrigger trigger) {
        _lookUpTrigger = trigger;
        _timer = 0;
    }

    public void LeaveLookUpTrigger(LookUpTrigger trigger) {
        if (trigger == _lookUpTrigger) {
            _lookUpTrigger = null;
        }
    }

    
    private void FireActionOnstarted(InputAction.CallbackContext obj) {
        Debug.Log("FireActionOnstarted");
        if (_playerInteractor!=null)_playerInteractor.Interact(this);
    }
    
    
}