using UnityEngine;

namespace HugoI.Scripts.Animation.Procedural
{
    public class LegController : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private float _speed = 10f;
        [SerializeField] private float _smoothTime = 0.1f;
        [SerializeField] private float _rotationSpeed = 90f;
        [SerializeField] private AnimationCurve _curveStep;
        [SerializeField] private float _stepHeight = 3f;

        [Header("Settings")]
        [SerializeField] private bool _smoothDamp = true;
        [SerializeField] private float _threshold = 2f;
        
        private ITarget _target;
        
        private Vector3 _currentVelocity;
        private float _currentStepVelocity;
        
        private Vector3 _startedPos;
        private Vector3 _currentTargetPos;
        private bool _isMoving;
        
        private void Awake()
        {
            _target = GetComponent<ITarget>();
        }

        private void Update()
        {
            if (Vector3.Distance(transform.position, _target.TargetPosition) >= _threshold && !_isMoving)
            {
                _isMoving = true;
                _currentTargetPos = _target.TargetPosition;
                _startedPos = transform.position;
            }
            
            if (_isMoving)
            {
                Vector3 startFlat = new Vector3(_startedPos.x, 0, _startedPos.z);
                Vector3 targetFlat = new Vector3(_currentTargetPos.x, 0, _currentTargetPos.z);
                Vector3 currentFlat = new Vector3(transform.position.x, 0, transform.position.z);

                float totalDistance = Vector3.Distance(startFlat, targetFlat);
                float currentDistance = Vector3.Distance(startFlat, currentFlat);
    
                float t = totalDistance > 0.01f ? Mathf.Clamp01(currentDistance / totalDistance) : 1f;
    
                // POSITION
                Vector3 nextPos;
                if (_smoothDamp)
                {
                    nextPos = Vector3.SmoothDamp(transform.position, _currentTargetPos,
                        ref _currentVelocity, _smoothTime, _speed, Time.deltaTime);
                }
                else
                {
                    nextPos = Vector3.MoveTowards(transform.position, _currentTargetPos, 
                        _speed * Time.deltaTime);
                }
    
                // HAUTEUR
                float baseHeight = Mathf.Lerp(_startedPos.y, _currentTargetPos.y, t);
                float arc = _curveStep.Evaluate(t) * _stepHeight;

                nextPos.y = baseHeight + arc;

                // APPLICATION
                transform.position = nextPos;

                // ARRÊT
                if (t >= 0.99f)
                {
                    _isMoving = false;
                    transform.position = _currentTargetPos;
                    _currentVelocity = Vector3.zero;
                }
            }
        }
    }
}