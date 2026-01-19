
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class AutoTurretController : MonoBehaviour , IDamagable
{
    [SerializeField] private Transform FireTarget;
    [SerializeField] private Transform _transformBody;
    [SerializeField] private Transform _transformCannons;

    [SerializeField] private Transform _target;
    [SerializeField] private float _bodyRotateSpeed = 40;
    [SerializeField] private float _bodyrotationThreshold = 0.97f;
    [SerializeField] private float _canonRotationThreshold = 0.95f;

    [Header("Life Parameters")]
    [SerializeField] private float _hp =5;
    [SerializeField] private GameObject _prfDeathGameObject;

    [Header("Fire Parameters")] 
    [SerializeField] private Transform _transformcannon1;
    [SerializeField] private Transform _transformcannon2;
    [SerializeField] private int _projectileDamage;
    [SerializeField] private float _projectileSpeed;
    [SerializeField] private float _fireDispertion = 1;
    [SerializeField] private float _fireRate = 1;
    [SerializeField] private Projectile _prfProjectile;
    [SerializeField] private float _agroRange = 15f;
    


    private Vector3 _targetVector { get => _target.position - _transformBody.position; }
    private Vector3 _targetVectorToCanon { get => _target.position - _transformCannons.position; }
    private bool _targetIsInRange { get => _targetVectorToCanon.magnitude < _agroRange; }

    private bool _canon1Fired;
    private PopoteTimer _fireTimer;
    

    void Start()
    {
        _fireTimer = new PopoteTimer(_fireRate / 1f);
        _fireTimer.Play();
        _fireTimer.OnTimerEnd += OnFireTimerEnd;
    }
    
    void Update() {
        if (_targetIsInRange) {
            ManageTurretMouvement();
            ManageFire();
        }
        
    }

    private float TopDownDotProduct(Vector3 reference, Vector3 target)
    {
        Vector2 a = new Vector2(reference.x, reference.z).normalized;
        Vector2 b = new Vector2(target.x, target.z).normalized;
        return Vector2.Dot(a, b);
    }
    
    private Vector3 GetRandomFireDirection()
    {
        return
            _transformCannons.forward +
                             _transformCannons.up * Random.Range(-_fireDispertion, _fireDispertion) +
                             _transformCannons.right * Random.Range(-_fireDispertion, _fireDispertion);
    }

    private Vector2 GetRandomPosInVector2(float radius)
    {
        float r = radius * Mathf.Sqrt(Random.Range(0f, 1f));
        float theta = Random.Range(0f, 1f) * 2 * Mathf.PI;
        Debug.Log(" R = " + r + "     theta =" + theta);
        return new Vector2(Mathf.Sin(theta) * r, Mathf.Cos(theta) * r);
    }

    private void ManageTurretMouvement() {
        if (TopDownDotProduct(_transformBody.forward, _targetVector.normalized) < _bodyrotationThreshold)
        {
            if (TopDownDotProduct(-_transformBody.right, _targetVector.normalized) < 0)
                _transformBody.Rotate(Vector3.up, _bodyRotateSpeed * Time.deltaTime);
            else
                _transformBody.Rotate(Vector3.up, -_bodyRotateSpeed * Time.deltaTime);
        }

        if (TopDownDotProduct(_transformBody.forward, _targetVector.normalized) > _canonRotationThreshold)
        {
            _transformCannons.forward = _targetVectorToCanon;
        }
    }

    private void ManageFire()
    {
        if (TopDownDotProduct(_transformBody.forward, _targetVector.normalized) > _canonRotationThreshold) {
            _fireTimer.UpdateTimer();
        }
    }

    private void OnFireTimerEnd(object sender, EventArgs e)
    {
        Projectile projectile;
        if (_canon1Fired) {
            projectile = Instantiate(_prfProjectile, _transformcannon2.position, Quaternion.identity);
            projectile.transform.forward = _transformcannon2.forward;
            _canon1Fired = false;
        }
        else {
            projectile = Instantiate(_prfProjectile, _transformcannon1.position, Quaternion.identity);
            projectile.transform.forward = _transformcannon1.forward;
            _canon1Fired = true;
        }
        projectile.SetUpProjectile(_projectileDamage, GetRandomFireDirection().normalized*_projectileSpeed);
        if( _targetIsInRange)_fireTimer.Play();
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.brown;
        Gizmos.DrawWireSphere(_transformBody.position, _agroRange);
    }

    public void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitNormal) {
        _hp-=damage;
        if (_hp <= 0) {
            if (_prfDeathGameObject != null) {
                Instantiate(_prfDeathGameObject, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }
}