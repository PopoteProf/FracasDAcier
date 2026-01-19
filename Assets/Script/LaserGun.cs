using UnityEngine;

public class LaserGun : Weapon
{
    
    [SerializeField, ColorUsage(true, true)] private Color _defaultLazerColor;
    [SerializeField, ColorUsage(true, true)] private Color _fireLazerColor;
    [SerializeField] private GameObject _prfFireImpact;
    [SerializeField] private int _damage =1;
    
    private bool _isFire;
    
    void Start() {
        _aimLineRenderer.startColor =_defaultLazerColor;
        _aimLineRenderer.endColor =_defaultLazerColor;
    }

    
    
    public override void StartClick() {
        if( _prfFireImpact){ 
            GameObject go = Instantiate(_prfFireImpact, hit.point, Quaternion.identity);
            go.transform.up = hit.normal;
        }

        if (hit.collider.GetComponent<IDamagable>() != null) {
            IDamagable target = hit.collider.GetComponent<IDamagable>();
            target.TakeDamage(_damage, hit.point, hit.normal);
        }
        _aimLineRenderer.startColor =_fireLazerColor;
        _aimLineRenderer.endColor =_fireLazerColor;
    }

    public override void StopClick()
    {
        _aimLineRenderer.startColor = _defaultLazerColor;
        _aimLineRenderer.endColor = _defaultLazerColor;
    }
}