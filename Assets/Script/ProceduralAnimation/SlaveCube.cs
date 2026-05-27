using UnityEngine;

public class SlaveCube : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _distanceToTarget;

    private void Update() {
        if (Vector3.Distance(transform.position, _target.position) > _distanceToTarget) {
            transform.position = _target.position-(_target.position-transform.position).normalized * _distanceToTarget;
            transform.forward = _target.position - transform.position;
        }
    }
}