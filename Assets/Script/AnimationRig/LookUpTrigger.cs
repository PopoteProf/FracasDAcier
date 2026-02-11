using UnityEngine;

public class LookUpTrigger : MonoBehaviour 
{
    [SerializeField] private Transform _lookUpTarget;

    public Transform LookUpTarget => _lookUpTarget;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
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
}