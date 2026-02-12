using UnityEngine;

public class PreceduralMovement : MonoBehaviour
{
    [SerializeField] private MouseManager _mouseManager;
    [Header("MoveTowards")]
    [SerializeField] private float _maxDistance;
    
    [Header("SmoothDamp")]
    [SerializeField] private float _smoothTime;
    [SerializeField] private float _maxSpeed;
    
    [Header("Raycast")]
    [SerializeField] private float _raycastDistance;
    [SerializeField] private float _heightOffset;
    
    private Vector3 _velocity;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!_mouseManager) _mouseManager =  FindObjectOfType<MouseManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //this.transform.position = Vector3.MoveTowards(this.transform.position,
        //    _mouseManager.movePosition, _maxDistance *  Time.deltaTime);        
        
        this.transform.position = Vector3.SmoothDamp(this.transform.position, _mouseManager.MovePosition,
            ref _velocity, _smoothTime, _maxSpeed, Time.deltaTime);
        
        this.transform.forward = Vector3.RotateTowards(this.transform.forward,
            _mouseManager.MovePosition - transform.position, (Mathf.PI / 4) * Time.deltaTime,
            _maxSpeed);
        
        if (!Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, _raycastDistance)) return;

        this.transform.position = hit.point + (Vector3.up * _heightOffset);
        
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.down * _raycastDistance);        
        
        Gizmos.color = Color.purple;
        Gizmos.DrawRay(transform.position, transform.forward * 5);
    }
}
