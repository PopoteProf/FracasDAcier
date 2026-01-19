using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]protected Transform _firePoint;
    [SerializeField]protected LineRenderer _aimLineRenderer;

    protected RaycastHit hit;
    protected bool _isEquipe;
    
    public virtual void StartClick()
    {
        
    }
    protected virtual void Update()
    {
        ManageAim();
    }
    protected virtual void ManageAim() {
        if (Physics.Raycast(new Ray(_firePoint.position, _firePoint.forward), out hit, Mathf.Infinity)) {
            _aimLineRenderer.SetPosition(0, _firePoint.position);
            _aimLineRenderer.SetPosition(1, hit.point);
        }
    }
    

    public virtual  void StopClick()
    {
        
    }

    public  virtual void ChangeSelection(bool isEquip) {
        _isEquipe = isEquip;
        gameObject.SetActive(isEquip);
        StopClick();
    }
}