using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class RaycastController : MonoBehaviour
{
    [SerializeField] Camera _cam;
    public static Vector3 _lastHitPoint;
    private bool _hasHit;

    void Update()
    {
        Ray ray = _cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        
        if (Mouse.current.leftButton.isPressed)
        {
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                _lastHitPoint = hit.point;
                _hasHit = true;
            }
        }
    }

    void OnDrawGizmos()
    {
        if (_hasHit)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_lastHitPoint, 0.2f);
        }
    }
}
