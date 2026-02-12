using UnityEngine;

public class ProceduralTesteur : MonoBehaviour
{
    [SerializeField] private Transform _target;
    
    
    [SerializeField] private float _speed;
    [SerializeField] private float _SmoothTime = 0.2f;
    [SerializeField] private float _rotateSpeed =3;

    private Vector3 _velocity;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        Debug.DrawLine(_target.position, transform.position, Color.green);
        Vector3 targetPos = Vector3.SmoothDamp(_target.position, transform.position,ref _velocity, _SmoothTime, _speed, Time.deltaTime);
        _target.position = targetPos;
        
        //Vector3 lookVec = Vector3.RotateTowards(_target.forward, (transform.position-_target.position).normalized, _rotateSpeed * Time.deltaTime, 0.0f);
        //_target.forward = lookVec;
        //Debug.DrawLine(_target.position, _target.position+_target.forward, Color.red);


        //RaycastHit hit;
        //if (Physics.Raycast(_target.position, _target.forward, out hit, Mathf.Infinity))
        //{
        //    float distance =hit.distance;
        //}
    }
}
