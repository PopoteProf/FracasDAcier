using UnityEngine;
using UnityEngine.Animations.Rigging;
using System.Collections;

public class LookUpTrigger : MonoBehaviour 
{
    [SerializeField] private Transform _lookUpTarget;
    [SerializeField] private float _smoothTime = 0.5f; // Adjustable in inspector

    public Transform LookUpTarget => _lookUpTarget;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        ThirdPersonCharacterController controller = other.GetComponent<ThirdPersonCharacterController>();
        controller.SetUpLookUpTrigger(this,  _lookUpTarget);
            
        MultiAimConstraint constraint = other.GetComponentInChildren<MultiAimConstraint>();
        if (!controller.LookUpTarget(_lookUpTarget))
        {
            controller.StopLookRoutine();
            controller.StartLookRoutine(SmoothToOriginal(constraint));
        }
            
        // Stop any existing coroutine and start new one
        controller.StopLookRoutine();
        controller.StartLookRoutine(SmoothToTarget(constraint));
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        ThirdPersonCharacterController controller = other.GetComponent<ThirdPersonCharacterController>();
        controller.LeaveLookUpTrigger(this);
        
        MultiAimConstraint constraint = other.GetComponentInChildren<MultiAimConstraint>();
        
        // Stop any existing coroutine and start new one
        controller.StopLookRoutine();
        controller.StartLookRoutine(SmoothToOriginal(constraint));
    }

    private IEnumerator SmoothToTarget(MultiAimConstraint constraint)
    {
        float elapsedTime = 0;
        
        while (elapsedTime < _smoothTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _smoothTime;
            
            WeightedTransformArray rig = constraint.data.sourceObjects;
            
            rig.SetWeight(0, Mathf.Lerp(1f, 0f, t));
            rig.SetWeight(1, Mathf.Lerp(0f, 1f, t));
            
            constraint.data.sourceObjects = rig;
            
            yield return null;
        }
        
        // Ensure final state
        WeightedTransformArray finalRig = constraint.data.sourceObjects;
        finalRig.SetWeight(0, 0f);
        finalRig.SetWeight(1, 1f);
        constraint.data.sourceObjects = finalRig;
    }

    public IEnumerator SmoothToOriginal(MultiAimConstraint constraint)
    {
        float elapsedTime = 0;
        
        while (elapsedTime < _smoothTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _smoothTime;
            
            WeightedTransformArray rig = constraint.data.sourceObjects;
            
            rig.SetWeight(0, Mathf.Lerp(0f, 1f, t));
            rig.SetWeight(1, Mathf.Lerp(1f, 0f, t));
            
            constraint.data.sourceObjects = rig;
            
            yield return null;
        }
        
        // Ensure final state
        WeightedTransformArray finalRig = constraint.data.sourceObjects;
        finalRig.SetWeight(0, 1f);
        finalRig.SetWeight(1, 0f);
        constraint.data.sourceObjects = finalRig;
    }
}