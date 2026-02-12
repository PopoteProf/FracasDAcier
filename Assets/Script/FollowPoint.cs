using UnityEngine;

public class FollowPoint : MonoBehaviour
{
    public float Speed;
    public float SmoothTime;
    [SerializeField] private bool _move = true;
    [SerializeField] private Vector3 _destination;
    [SerializeField] private GameObject _targetToFollow;
    Vector3 _velocity;
    
    private void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 3 , Color.blue, 0.1f);
        Vector3 LookVector = _destination - transform.position;
        if (_move)
        {
            transform.position = Vector3.SmoothDamp(transform.position, _destination, ref _velocity , SmoothTime * Time.deltaTime,  Speed);
            transform.forward = Vector3.RotateTowards(transform.forward, LookVector,10 * Mathf.Deg2Rad, 500);
            Quaternion rotation = transform.rotation;
            rotation.x = 0;
            transform.rotation = rotation;
            //transform.rotation = Quaternion.Euler(rotate);
        }
        
        Debug.DrawRay(transform.position, LookVector , Color.yellow);
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down * 100, out hit))
        {
            Vector3 position = transform.position;
            position.y = hit.point.y + 0.5f;
            transform.position = position;
        }
        _destination = _targetToFollow.transform.position;
    }
}
