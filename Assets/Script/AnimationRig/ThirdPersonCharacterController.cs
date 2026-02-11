using System.Collections;
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
    [SerializeField] private float _acceleration = 1;
    [SerializeField] private float _rotaitionSpeed = 90;
    [SerializeField] private Transform _defaultTarget;
    [SerializeField] private Transform _headTransform;
    [SerializeField] private float _smoothTime = 0.5f; // Adjustable in inspector
    [SerializeField] private MultiAimConstraint _multiAimConstraint;
    
    CharacterController _characterController;
    float _moveSpeed;
    LookUpTrigger _lookUpTrigger;
    Transform _target;
    private Transform _buttonTransform;

    InputAction _moveAction;
    InputAction _fireAction;
    
    private Coroutine _currentLookRoutine;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _moveAction = InputSystem.actions.FindAction("Move");

        _fireAction = InputSystem.actions.FindAction("Attack");
        _fireAction.started += FireActionOnstarted;
    }
    private void OnDisable()
    {
        StopLookRoutine();
    }


    private void Update()
    {
        ManageMouvement();

        if (!_target) return;

        if (!LookUpTarget(_target))
        {
            StopLookRoutine();
            StartLookRoutine(SmoothToOriginal());
        }
    }

    private void ManageMouvement()
    {
        Vector2 inputVec = Vector2.zero;
        if (_freezControl) inputVec = _moveAction.ReadValue<Vector2>();

        if (inputVec.y > 0.5f)
        {
            _moveSpeed += _acceleration * Time.deltaTime;
        }
        else if (inputVec.y < -0.5f)
        {
            _moveSpeed -= _acceleration * Time.deltaTime;
        }
        else
        {
            if (_moveSpeed != 0)
            {
                if (_moveSpeed > 0)
                {
                    _moveSpeed = Mathf.Clamp(_moveSpeed - _acceleration * Time.deltaTime, 0, 1);
                }

                if (_moveSpeed < 0)
                {
                    _moveSpeed = Mathf.Clamp(_moveSpeed + _acceleration * Time.deltaTime, -1, 0);
                }
            }
        }

        _moveSpeed = Mathf.Clamp(_moveSpeed, -1, 1);

        Vector3 moveVec = _moveSpeed * moveSpeed * Time.deltaTime * transform.forward;
        _characterController.Move(moveVec);
        if (_moveSpeed < 0) _animator.SetFloat("Speed", -_characterController.velocity.magnitude);
        else _animator.SetFloat("Speed", _characterController.velocity.magnitude);

        if (inputVec.x > 0.5f)
        {
            transform.Rotate(transform.up, _rotaitionSpeed * Time.deltaTime);
            _animator.SetFloat("Rotation", 1);
        }
        else if (inputVec.x < -0.5f)
        {
            transform.Rotate(transform.up, -_rotaitionSpeed * Time.deltaTime);
            _animator.SetFloat("Rotation", -1);
        }
        else
        {
            _animator.SetFloat("Rotation", 0);
        }
    }

    /// <summary>
    /// If the Dot product of the Vectors between the normal look target
    /// and the intended new target is larger than we want, then we will return false; it should stop the look at.
    /// </summary>
    /// <param name="target">Look at Target Transform</param>
    /// <returns></returns>
    public bool LookUpTarget(Transform target)
    {
        Vector3 head2Default = _defaultTarget.position - _headTransform.position;
        Vector3 head2LookTarget = _headTransform.position - target.position;
        float dotProduct = Vector3.Dot(head2Default.normalized, head2LookTarget.normalized);

        return dotProduct > 0.5 || dotProduct < -0.5;
    }
    
    public void SetUpLookUpTrigger(LookUpTrigger trigger, Transform target)
    
    {
        _lookUpTrigger = trigger;
        _target = target;
    }

    public void LeaveLookUpTrigger(LookUpTrigger trigger)
    {
        if (trigger != _lookUpTrigger) return;
        
        _lookUpTrigger = null;
        _target = null;
    }
    public void StartLookRoutine(IEnumerator routine)
    {
        _currentLookRoutine = StartCoroutine(routine);
    }
    
    public void StopLookRoutine()
    {
        if (_currentLookRoutine == null) return;
        
        StopCoroutine(_currentLookRoutine);
        _currentLookRoutine = null;
    }

    public IEnumerator SmoothToOriginal( )
    {
        float elapsedTime = 0;
        
        while (elapsedTime < _smoothTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _smoothTime;
            
            WeightedTransformArray rig = _multiAimConstraint.data.sourceObjects;
            
            rig.SetWeight(0, Mathf.Lerp(0f, 1f, t));
            rig.SetWeight(1, Mathf.Lerp(1f, 0f, t));
            
            _multiAimConstraint.data.sourceObjects = rig;
            
            yield return null;
        }
        
        // Ensure final state
        WeightedTransformArray finalRig = _multiAimConstraint.data.sourceObjects;
        finalRig.SetWeight(0, 1f);
        finalRig.SetWeight(1, 0f);
        _multiAimConstraint.data.sourceObjects = finalRig;
    }

    private void FireActionOnstarted(InputAction.CallbackContext obj)
    {
        Debug.Log("FireActionOnstarted");
        if (_playerInteractor != null) _playerInteractor.Interact(this);
    }
}