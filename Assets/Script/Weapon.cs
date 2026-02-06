using System.Collections.Generic;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.VFX;

public class Weapon : MonoBehaviour
{
    [SerializeField]protected Vector3 _scale;
    [SerializeField]protected Transform _firePoint;
    [SerializeField]protected LineRenderer _aimLineRenderer;
    [SerializeField]protected CinemachineImpulseSource _fireImpulseSource;
    [SerializeField]protected GameObject _prfMuzzleFire;
    [SerializeField]protected List<VisualEffect> _visualEffects;
    

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
        if (isEquip)
        {
            gameObject.SetActive(isEquip);
            transform.DOScale(_scale, 0.5f);
        }
        else
        {
            transform.DOScale(0f, 0.5f).OnComplete(() => {gameObject.SetActive(isEquip);});
        }
        StopClick();
    }
    
    public List<VisualEffect> GetVisualEffects() {
        return _visualEffects;
    }
}