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

    [Header("LookUpTarget")] [SerializeField] private MultiAimConstraint _aimConstraint;
    [SerializeField] private Transform _lookUpTarget;
    [SerializeField] private float _lookUpDotThreashold = 0.5f;
    [SerializeField] private float _lookUpAcceleration = 1f;

    [Header("LeftArmRayCast")]
    [SerializeField] private Transform _leftArmRayCaster;
    [SerializeField] private float _leftArmThreashold = 1f;
    [SerializeField] private TwoBoneIKConstraint _LeftArmTwoBoneIKConstraint;
    [SerializeField] private AnimationCurve _LeftArmAnimationCurveWeight = AnimationCurve.EaseInOut(0,0,1,1);
    [SerializeField] private Transform _LeftArmTwoBoneIKTarget;
    [SerializeField] private Vector3 _LeftTargetPositionOffset;
    [SerializeField] private Vector3 _LeftTargetrotationOffset;
    [SerializeField] private float _leftArmweightAcceleration =3;
    [Header("RightArmRayCast")]
    [SerializeField] private TwoBoneIKConstraint _rightArmRayTwoBoneIKConstraint;
    [SerializeField] private Transform _rightArmTwoBoneIKTarget;
    [SerializeField] private float _buttonAnimationTimer;
    
    
    CharacterController _characterController;
    float _moveSpeed;
    LookUpTrigger _lookUpTrigger;
    private PopoteTimer _buttonTimer;
    private Transform _buttonTransform;
    
    InputAction _moveAction;
    InputAction _fireAction;
    
    void Start() {
        _characterController =  GetComponent<CharacterController>();
        _moveAction = InputSystem.actions.FindAction("Move");
        
        _fireAction = InputSystem.actions.FindAction("Attack");
        _fireAction.started += FireActionOnstarted;
        _buttonTimer = new PopoteTimer(_buttonAnimationTimer);

    }

    

    private void Update() {
        ManageMouvement();
        ManageLookUp();
        ManagerLeftArmWall();
        if (_buttonTimer.IsPlaying) ManagerButtonAnimation();
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
    }

    public void LeaveLookUpTrigger(LookUpTrigger trigger) {
        if (trigger == _lookUpTrigger) {
            _lookUpTrigger = null;
        }
    }

    private void ManageLookUp() {
        if (_lookUpTrigger == null) {
            _aimConstraint.weight = Mathf.Clamp(_aimConstraint.weight-_lookUpAcceleration*Time.deltaTime, 0,1);
            return;
        }
        _lookUpTarget.position = _lookUpTrigger.LookUpTarget.position;

        if (Vector3.Dot(transform.forward,
                (_lookUpTarget.position - _aimConstraint.gameObject.transform.position).normalized) <
            _lookUpDotThreashold) {
            _aimConstraint.weight = Mathf.Clamp(_aimConstraint.weight-_lookUpAcceleration*Time.deltaTime, 0,1);    
        }
        else {
            _aimConstraint.weight = Mathf.Clamp(_aimConstraint.weight+_lookUpAcceleration*Time.deltaTime, 0,1);  
        }
    }

    private void ManagerLeftArmWall()
    {
        if (Physics.Raycast(new Ray(_leftArmRayCaster.position, _leftArmRayCaster.forward), out RaycastHit hit)) {
            
            //_LeftArmTwoBoneIKConstraint.weight = 1;
            if (hit.distance <= _leftArmThreashold) {
                _LeftArmTwoBoneIKTarget.position = hit.point;
                _LeftArmTwoBoneIKTarget.transform.forward = -hit.normal;
                _LeftArmTwoBoneIKTarget.localPosition+= _LeftTargetPositionOffset;
                _LeftArmTwoBoneIKTarget.localEulerAngles += _LeftTargetrotationOffset;
                
                float targetWeight = _LeftArmAnimationCurveWeight.Evaluate(1-(hit.distance/_leftArmThreashold));
                if (_LeftArmTwoBoneIKConstraint.weight < targetWeight) {
                    _LeftArmTwoBoneIKConstraint.weight  =Mathf.Clamp(_LeftArmTwoBoneIKConstraint.weight+_leftArmweightAcceleration*Time.deltaTime,0,targetWeight);
                }
                else {
                    _LeftArmTwoBoneIKConstraint.weight  =Mathf.Clamp(_LeftArmTwoBoneIKConstraint.weight-_leftArmweightAcceleration*Time.deltaTime,targetWeight,1);
                }
            }
            else
            {
                _LeftArmTwoBoneIKConstraint.weight  =Mathf.Clamp(_LeftArmTwoBoneIKConstraint.weight-_leftArmweightAcceleration*Time.deltaTime,0,1);
            }
        }
        else
        {
            _LeftArmTwoBoneIKConstraint.weight  =Mathf.Clamp(_LeftArmTwoBoneIKConstraint.weight-_leftArmweightAcceleration*Time.deltaTime,0,1);
        }
    }
    private void FireActionOnstarted(InputAction.CallbackContext obj) {
        Debug.Log("FireActionOnstarted");
        if (_playerInteractor!=null)_playerInteractor.Interact(this);
    }
    public void PlayButtonAnimation(Transform buttonTransform) {
        _animator.SetTrigger("Button");
        _buttonTimer.Play();
        _buttonTransform = buttonTransform;
    }

    private void ManagerButtonAnimation() {
        _rightArmTwoBoneIKTarget.transform.position = _buttonTransform.position;
        _rightArmRayTwoBoneIKConstraint.weight = _animator.GetFloat("ButtonIK");
        _buttonTimer.UpdateTimer();
    }
}