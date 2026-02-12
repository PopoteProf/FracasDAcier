using UnityEngine;
using UnityEngine.InputSystem;

namespace HugoI.Scripts.Animation.Procedural
{
    public class RaycastController : MonoBehaviour, ITarget
    {
        [Header("References")]
        [SerializeField] private Camera _camera;
        
        public Vector3 TargetPosition { get; set; }
        
        private void Update()
        {
            if (Mouse.current.leftButton.isPressed)
            {
                Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
                bool hitSomething = Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Ground"));
                
                if (hitSomething)
                {
                    TargetPosition = hit.point;
                    Debug.DrawLine(ray.origin, hit.point, Color.red);
                }
                else
                {
                    TargetPosition = Vector3.zero;
                }
            }
        }
    }
}