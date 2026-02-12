using UnityEngine;
using UnityEngine.InputSystem;

public class RayCastController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    public Vector3 Target;
    private Vector3 _mousePosition;
    private Ray _ray;

    private bool _isPressed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _isPressed = Mouse.current.leftButton.isPressed;
        _mousePosition = Mouse.current.position.ReadValue();
        _ray = _camera.ScreenPointToRay(_mousePosition);
        if (_isPressed)
        {
            if (Physics.Raycast(_ray, out RaycastHit hit))
            {
                Target = hit.point;
            }
        }
    }
}
