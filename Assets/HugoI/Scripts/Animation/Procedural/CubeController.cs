using UnityEngine;

namespace HugoI.Scripts.Animation.Procedural
{
    public class CubeController : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private float _speed = 10f;
        [SerializeField] private float _smoothTime = 0.5f;
        [SerializeField] private float _rotationSpeed = 90f;
        
        [Header("Settings")]
        [SerializeField] private bool _smoothDamp = true;
        
        private ITarget _target;
        
        private Vector3 _currentVelocity;

        private void Awake()
        {
            _target = GetComponent<ITarget>();
        }

        private void Update()
        {
            if (_target.TargetPosition != Vector3.zero)
            {
                // POSITION
                Vector3 movement = Vector3.zero;

                if (_smoothDamp)
                {
                    movement = Vector3.SmoothDamp(transform.position, _target.TargetPosition,
                        ref _currentVelocity, _smoothTime, _speed, Time.deltaTime);
                }
                else
                {
                    movement = Vector3.MoveTowards(transform.position, _target.TargetPosition, 
                        _speed * Time.deltaTime);
                }
                
                transform.position = movement;
                
                // ROTATION
                Vector3 direction = (_target.TargetPosition - transform.position).normalized;
                Vector3 rotation = Vector3.RotateTowards(transform.forward, direction, 
                    _rotationSpeed * Mathf.Deg2Rad * Time.deltaTime, 0.0f);
            
                transform.forward = rotation;
            }
            
            // POSITION Y
            Physics.Raycast(transform.position + Vector3.up * 5, Vector3.down * 10, out RaycastHit hit, 
                Mathf.Infinity, LayerMask.GetMask("Ground"));

            var vector3 = transform.position;
            vector3.y = hit.point.y;
            transform.position = vector3;
        }
    }
}