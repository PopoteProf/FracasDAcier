using UnityEngine;
using UnityEngine.VFX;

namespace Hugo_I.Scripts
{
    public class DestroyAfterTime : MonoBehaviour
    {
        [SerializeField] private float _time;

        private VisualEffect _visualEffect;

        private void Awake()
        {
            _visualEffect = GetComponent<VisualEffect>();
        }

        private void OnEnable()
        {
            Invoke(nameof(StopVisualEffect), _time);
            Destroy(gameObject, _time * 2f);
        }

        private void StopVisualEffect()
        {
            _visualEffect.Stop();
        }
    }
}
