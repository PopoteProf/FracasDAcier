using UnityEngine;

public class SelfDestroy : MonoBehaviour {
    [SerializeField] private float _timeToDestroy;
    private void Start() {
        Destroy(gameObject, _timeToDestroy);
    }
}