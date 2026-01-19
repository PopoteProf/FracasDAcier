using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    private Vector3 _previousDisplacement = Vector3.zero;
    private Vector3 _previousVelocity = Vector3.zero;

    [Tooltip("The local position about which oscillations are centered.")]
    public Vector3 localEquilibriumPosition = Vector3.zero;
    [Tooltip("The axes over which the oscillator applies force. Within range [0, 1].")]
    public Vector3 forceScale = Vector3.one;
    [Tooltip("The greater the stiffness constant, the lesser the amplitude of oscillations.")]
    [SerializeField] private float _stiffness = 100f;
    [Tooltip("The greater the damper constant, the faster that oscillations will dissapear.")]
    [SerializeField] private float _damper = 2f;
    [Tooltip("The greater the mass, the lesser the amplitude of oscillations.")]
    [SerializeField] private float _mass = 1f;

    private void FixedUpdate()
    {
        Vector3 restoringForce = CalculateRestoringForce();
        ApplyForce(restoringForce);
    }

    private Vector3 CalculateRestoringForce()
    {
        Vector3 displacement = transform.localPosition - localEquilibriumPosition; // Displacement from the rest point. Displacement is the difference in position.
        Vector3 deltaDisplacement = displacement - _previousDisplacement;
        _previousDisplacement = displacement;
        Vector3 velocity = deltaDisplacement / Time.fixedDeltaTime; // Kinematics. Velocity is the change-in-position over time.
        Vector3 force = HookesLaw(displacement, velocity);
        return (force);
    }

    private Vector3 HookesLaw(Vector3 displacement, Vector3 velocity)
    {
        Vector3 force = (_stiffness * displacement) + (_damper * velocity); // Damped Hooke's law
        force = -force; // Take the negative of the force, since the force is restorative (attractive)
        return (force);
    }

    public void ApplyForce(Vector3 force)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(Vector3.Scale(force, forceScale)); 
        }
        else
        {
            Vector3 displacement = CalculateDisplacementDueToForce(force);
            transform.localPosition += Vector3.Scale(displacement, forceScale);
        }
    }
    private Vector3 CalculateDisplacementDueToForce(Vector3 force)
    {
        Vector3 acceleration = force / _mass; // Newton's second law.
        Vector3 deltaVelocity = acceleration * Time.fixedDeltaTime; // Kinematics. Acceleration is the change in velocity over time.
        Vector3 velocity = deltaVelocity + _previousVelocity; // Calculating the updated velocity.
        _previousVelocity = velocity;
        Vector3 displacement = velocity * Time.fixedDeltaTime; // Kinematics. Velocity is the change-in-position over time.
        return (displacement);
    }

    
}