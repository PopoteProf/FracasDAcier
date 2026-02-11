using UnityEngine;
using UnityEngine.InputSystem;

public class RootMotionController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _acceleration =1;
    [SerializeField] private float RotaitionSpeed =90;
    InputAction _moveAction;
    float _moveSpeed;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _moveAction = InputSystem.actions.FindAction("Move");
    }

    // Update is called once per frame
    void Update()
    {
        ManageMouvement();
    }

    private void ManageMouvement() {
        Vector2 movevalue =_moveAction.ReadValue<Vector2>();
        if (movevalue.y > 0.5f) {
            _moveSpeed += _acceleration * Time.deltaTime;
        }
        else if (movevalue.y < -0.5f) {
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
        _animator.SetFloat("Speed", _moveSpeed);
        
        if( movevalue.x>0.5f)transform.Rotate(transform.up, RotaitionSpeed*Time.deltaTime);
        if( movevalue.x<-0.5f)transform.Rotate(transform.up, -RotaitionSpeed*Time.deltaTime);
    }
}