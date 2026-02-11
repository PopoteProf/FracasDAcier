using UnityEngine;

public class LookUpTrigger : MonoBehaviour {
    [SerializeField]private Transform _lookUpTarget;

    public Transform LookUpTarget => _lookUpTarget;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            other.GetComponent<ThirdPersonCharacterController>().SetUpLookUpTrigger(this);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        { 
            other.GetComponent<ThirdPersonCharacterController>().LeaveLookUpTrigger(this);
        }
    }
    
    void OnDrawGizmos()
    {
        SphereCollider col = GetComponent<SphereCollider>();

        if (col == null) return;

        Gizmos.color = Color.yellow;

        // Attention au center local !
        Vector3 worldCenter = transform.TransformPoint(col.center);

        Gizmos.DrawWireSphere(worldCenter, col.radius * transform.lossyScale.x);
    }
}