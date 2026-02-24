using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCubeController : NetworkTransform
{
#if UNITY_EDITOR
    // These bool properties ensure that any expanded or collapsed property views
    // within the inspector view will be saved and restored the next time the
    // asset/prefab is viewed.
    public bool PlayerCubeControllerPropertiesVisible;
#endif
    public float Speed = 10;
    public bool ApplyVerticalInputToZAxis;
    private Vector3 m_Motion;
    private InputAction _moveActionMap;

    private void Start()
    {
        _moveActionMap =  InputSystem.actions.FindAction("Move");
    }
    
    private void Update()
    {
        // If not spawned or we don't have authority, then don't update
        if (!IsSpawned || !HasAuthority)
        {
            return;
        }

        // Handle acquiring and applying player input
        m_Motion = Vector3.zero;
        m_Motion.x = _moveActionMap.ReadValue<Vector2>().x;

        // Determine whether the vertical input is applied to the Y or Z axis
        if (!ApplyVerticalInputToZAxis)
        {
            m_Motion.y =_moveActionMap.ReadValue<Vector2>().y;
        }
        else
        {
            m_Motion.z = _moveActionMap.ReadValue<Vector2>().y;
        }

        // If there is any player input magnitude, then apply that amount of
        // motion to the transform
        if (m_Motion.magnitude > 0)
        {
            transform.position += m_Motion * Speed * Time.deltaTime;
        }
    }
}