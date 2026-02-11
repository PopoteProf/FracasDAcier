using HugoI.Scripts.Animation;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonCharacterController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private PlayerInteractor _playerInteractor;
    [SerializeField] private bool _freezControl;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _acceleration =1;
    [SerializeField] private float RotaitionSpeed =90;

    [Header("Multi-Aim Constraint")]
    [SerializeField] private MultiAimConstraint _multiAimConstraint;
    [SerializeField] private AnimationCurve _animationCurveAim;
    [SerializeField] private float _lookUpSpeed;
    
    [Header("Two Bone IK Constraint")]
    [SerializeField] private TwoBoneIKConstraint _twoBoneIKConstraint;
    [SerializeField] private AnimationCurve _animationCurveTwoBone;
    [SerializeField] private float _hitWallSpeed;
    
    private CharacterController _characterController;
    private float _moveSpeed;
    
    // LookUp
    private LookUpTrigger _lookUpTrigger;
    private float _timeAnimationLookUp;
    
    // HitWall
    private HitWall _hitWallRight;
    private float _timeAnimationTwoBoneRight;
    
    private Transform _buttonTransform;
    
    private InputAction _moveAction;
    private InputAction _fireAction;
    
    void Start() {
        _characterController =  GetComponent<CharacterController>();
        _moveAction = InputSystem.actions.FindAction("Move");
        
        _fireAction = InputSystem.actions.FindAction("Attack");
        _fireAction.started += FireActionOnstarted;

    }

    private void Update() 
    {
        ManageMouvement();
        ManageLookUp();
    }

    private void ManageMouvement() 
    {
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

    private void ManageLookUp()
    {
        if (_lookUpTrigger)
        {
            _multiAimConstraint.data.sourceObjects[0].transform.position = _lookUpTrigger.LookUpTarget.position;
            
            if (_multiAimConstraint.weight >= 1f) return;
            
            _timeAnimationLookUp += Mathf.Clamp(Time.deltaTime * _lookUpSpeed, 0f, 1f);
            _multiAimConstraint.weight = Mathf.Lerp(0f, 1f, _animationCurveAim.Evaluate(_timeAnimationLookUp));
        }
        else
        {
            if (_multiAimConstraint.weight <= 0f) return;
            
            _timeAnimationLookUp -= Mathf.Clamp(Time.deltaTime * _lookUpSpeed, 0f, 1f);;
            _multiAimConstraint.weight = Mathf.Lerp(0f, 1f, _animationCurveAim.Evaluate(_timeAnimationLookUp));
        }
    }

    // LookUp
    public void SetUpLookUpTrigger(LookUpTrigger trigger) 
    {
        _lookUpTrigger = trigger;
        _timeAnimationLookUp = 0f;
    }

    public void LeaveLookUpTrigger(LookUpTrigger trigger) 
    {
        if (trigger == _lookUpTrigger) 
        {
            _lookUpTrigger = null;
        }
    }
    
    // HitWall
    public void HitWallRight(HitWall hitWall)
    {
        _hitWallRight = hitWall;

        if (_hitWallRight.HitWallPos != Vector3.zero)
        {
            _twoBoneIKConstraint.data.target.position = _hitWallRight.HitWallPos;
            
            if (_twoBoneIKConstraint.weight >= 1f) return;
            
            _timeAnimationTwoBoneRight += Mathf.Clamp(Time.deltaTime * _hitWallSpeed, 0f, 1f);
            _twoBoneIKConstraint.weight = Mathf.Lerp(0f, 1f, _animationCurveTwoBone.Evaluate(_timeAnimationTwoBoneRight));
        }
        else
        {
            if (_twoBoneIKConstraint.weight <= 0f) return;
            
            _timeAnimationTwoBoneRight -= Mathf.Clamp(Time.deltaTime * _hitWallSpeed, 0f, 1f);;
            _twoBoneIKConstraint.weight = Mathf.Lerp(0f, 1f, _animationCurveTwoBone.Evaluate(_timeAnimationTwoBoneRight));
        }
    }
    
    private void FireActionOnstarted(InputAction.CallbackContext obj) {
        Debug.Log("FireActionOnstarted");
        if (_playerInteractor!=null)_playerInteractor.Interact(this);
    }
    
    
}