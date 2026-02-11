using UnityEngine;

namespace HugoI.Scripts.Animation
{
    [ExecuteAlways]
    public class HitWall : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _maxDistanceHit;
        
        [Header("References")]
        [SerializeField] private ThirdPersonCharacterController _thirdPersonCharacterController;
        
        [Header("Debug")]
        [SerializeField] private bool _haveDebug;

        public Vector3 HitWallPos => _hitWallPos;
        private Vector3 _hitWallPos;
        
        private void Update()
        {
            Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _maxDistanceHit, 
                LayerMask.GetMask("Ground"));
            if (_haveDebug) Debug.DrawRay(transform.position, transform.forward * _maxDistanceHit, Color.red);

            _hitWallPos = hit.point;

            _thirdPersonCharacterController.HitWallRight(this);
        }
    }
}