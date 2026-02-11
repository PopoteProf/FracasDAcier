using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour {
    
    [SerializeField]private bool _canInteract = true;
    private IInteractable _currentInteractable;
    private List<IInteractable> _interactables = new List<IInteractable>();

    public void Interact(ThirdPersonCharacterController player)
    {
        if (!_canInteract) return;
        if (_currentInteractable!=null) _currentInteractable.Interact(player);
    }
    
    public void SetCanInteract(bool value) {
        
        _canInteract = value;
        CheckForCurrentInteractable();
    }
    private void OnTriggerEnter(Collider other) {
        Debug.Log(other.gameObject.name);
        if (other.GetComponent<IInteractable>() != null) {
            AddInteractable(other.GetComponent<IInteractable>());
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.GetComponent<IInteractable>() != null) {
            RemoveIneractable(this, other.GetComponent<IInteractable>());
        } 
    }

    private void Update() {
        if (_interactables.Count > 0) {
            CheckForCurrentInteractable();
        }
    }

    private void AddInteractable(IInteractable interactable) {
        if (interactable == null) return;
        if (_interactables.Contains(interactable)) return;
        if (!interactable.CanInteract()) return;
        _interactables.Add(interactable);
        interactable.SetInteractable(true);
        interactable.OnInteractableAutoRemove+= RemoveIneractable;
        CheckForCurrentInteractable();
    }

    private void RemoveIneractable(object sender, IInteractable e) {
        if (!_interactables.Contains(e)) return;
        _interactables.Remove(e);
        e.SetInteractable(false);
        e.OnInteractableAutoRemove -= RemoveIneractable;
        if (e == _currentInteractable) {
            _currentInteractable.SetIsPrimaryInteractable(false);
            _currentInteractable = null;
        }
        CheckForCurrentInteractable();
    }

    private void CheckForCurrentInteractable() {
        if( _interactables.Count==0) return;
        if (!_canInteract) {
            foreach (IInteractable interactable in _interactables) {
                interactable.SetIsPrimaryInteractable(false);
            }
        }
        else
        {
            float bestdistance = _interactables[0].GetToDistance(transform.position);
            IInteractable bestinteractable = _interactables[0];
            foreach (IInteractable interactable in _interactables) {
                if (interactable.GetToDistance(transform.position) < bestdistance) {
                    bestinteractable = interactable;
                    bestdistance = interactable.GetToDistance(transform.position);
                }
            }

            if (bestinteractable != _currentInteractable) {
                if (_currentInteractable != null) _currentInteractable.SetIsPrimaryInteractable(false);
                _currentInteractable = bestinteractable;
                _currentInteractable.SetIsPrimaryInteractable(true);
            }
        }
    }
}