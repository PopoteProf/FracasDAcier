using System;
using UnityEngine;

public class ButtonInteractor : MonoBehaviour, IInteractable
{
    [SerializeField] private bool _isInteractable = true;
    [SerializeField] private GameObject _interactableUI;
    [SerializeField] private GameObject _currentInteractableUI;
    [SerializeField] private GameObject _prfParticuleSistem;
    [SerializeField] private Transform _buttonTransform;
    
    public event EventHandler<IInteractable> OnInteractableAutoRemove;
    public float GetToDistance(Vector3 playerPosition) => Vector3.Distance(transform.position, playerPosition);
    public bool CanInteract() => _isInteractable;

    public void SetInteractable(bool value) {
        _interactableUI.SetActive(value);
        if( !value)_currentInteractableUI.SetActive(false);
    }

    public void SetIsPrimaryInteractable(bool value)=> _currentInteractableUI.SetActive(value);
    public void Interact(ThirdPersonCharacterController player) {
        Instantiate(_prfParticuleSistem, transform.position, Quaternion.identity);
    }
}