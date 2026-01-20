using UnityEngine;

namespace Script
{
    public class SlimeCollider :MonoBehaviour
    {
        [SerializeField] private bool _canGlue;
        [SerializeField] private bool _isGlue;
        [SerializeField] private float _timeBeforGlue;
        private Vector3 _lastPos;
        public float radius = 0.5f;
        public LayerMask wallLayer;

        private void Start()
        {
            _lastPos = transform.position;
        }
        private void OnTriggerEnter(Collider other)
        {
            
            if (_canGlue)
            {
               
            }
        }

        protected void Update()
        {
            if (_isGlue) return;
            
            RaycastHit hit;
        
            if (Physics.Raycast(new Ray(_lastPos, transform.position - _lastPos), out hit, (transform.position - _lastPos).magnitude))
            {
                Impact(hit);
            }
            _lastPos = transform.position;
            
            _timeBeforGlue -= Time.deltaTime;
            if (_timeBeforGlue <= 0)
            {
                _canGlue = true;
            }
        }

        protected void Impact(RaycastHit hit)
        {
            if (_canGlue)
            {
                transform.position = hit.point;
                transform.parent = null;
                _isGlue = true;
                Debug.Log("TOUCHE"); 
            }
        }
    }
}
