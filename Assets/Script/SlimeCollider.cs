using System;
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

        public float FallingSpeed;
        public float SlidingSpeed;
        
        public float TimeBeforDestroy;
        public float Currentime;
        private void Start()
        {
            _lastPos = transform.position;
            _scale = transform.localScale;
        }

        private void OnTriggerExit(Collider other)
        {
            Debug.Log(other.gameObject.name);
        }

        protected void Update()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 0.2f);
            Vector3 startPos = new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z);
            Debug.DrawLine(startPos, new Vector3(transform.position.x, transform.position.y - 0.08f, transform.position.z), Color.blue, Time.deltaTime);
            if (colliders.Length > 0 && !Physics.Linecast(transform.position, new Vector3(transform.position.x, transform.position.y - 0.08f, transform.position.z)))
            {
                Vector3 position = transform.position;
                position = new Vector3(position.x, position.y -= SlidingSpeed * Time.deltaTime, position.z);
                transform.position = position;
            }
            else if (!Physics.Linecast(transform.position, new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z)))
            {
                Vector3 position = transform.position;
                position = new Vector3(position.x, position.y -= FallingSpeed * Time.deltaTime, position.z);
                transform.position = position;
            }

            Currentime += Time.deltaTime;
            if (Currentime >= TimeBeforDestroy)
            {
                Vector3 scale = transform.localScale;
                scale = new Vector3(scale.x -= 0.08f * Time.deltaTime, scale.y -= 0.08f * Time.deltaTime, scale.z -= 0.08f * Time.deltaTime);
                transform.localScale = scale;
                if (scale.x <= 0)
                {
                    Destroy(gameObject);
                }
            }
            
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
