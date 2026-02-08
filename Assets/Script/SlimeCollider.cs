using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

namespace Script
{
    public class SlimeCollider :MonoBehaviour
    {
        [SerializeField] private bool _canGlue;
        [SerializeField] private bool _isGlue;
        [SerializeField] private float _timeBeforGlue;
        [SerializeField] private VisualEffect  _visualEffect;
        private Vector3 _scale;
        private Vector3 _lastPos;
        public float radius = 0.5f;
        public LayerMask layerIngnor;

        private void Start()
        {
            _lastPos = transform.position;
            _scale = transform.localScale;
        }
        private void OnTriggerEnter(Collider other)
        {
            
        }

        protected void Update()
        {
            if (_isGlue) return;
            
            RaycastHit hit;
        
            if (Physics.Raycast(new Ray(_lastPos, transform.position - _lastPos), out hit, (transform.position - _lastPos).magnitude) && !hit.transform.gameObject.CompareTag("Player"))
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
                _visualEffect.Play();
                transform.localScale = _scale;
                StartCoroutine("DestroyVFX");
            }
        }

        public IEnumerator DestroyVFX()
        {
            yield return new WaitForSeconds(10f);
            Destroy(_visualEffect);
        }
    }
}
