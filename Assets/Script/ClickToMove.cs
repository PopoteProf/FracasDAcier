using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClickToMove : MonoBehaviour
{
    public Camera Camera;

    private void OnEnable()
    {
        throw new NotImplementedException();
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            OnClick();  
        }
    }

    public void OnClick()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.ScreenPointToRay(mousePos);
        
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 1f);
        if (Physics.Raycast(ray, out hit))
        {
            EventBus.OnPlayerClickedOnGround?.Invoke(hit.point);
        }
    }
}
