using UnityEngine;

namespace HugoI.Scripts.Animation.Procedural
{
    public class TargetController : MonoBehaviour, ITarget
    {
        [Header("References")]
        [SerializeField] private Transform _target;
        
        public Vector3 TargetPosition { get; set; }
        
        private void Update()
        {
            TargetPosition = _target.position;
        }
    }
}