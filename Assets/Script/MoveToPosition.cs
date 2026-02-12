using UnityEngine;

public class MoveToPosition : MonoBehaviour
{
    public float Speed;
    public float SmoothTime;
    [SerializeField] private Vector3 _destination;
    Vector3 _velocity;
    
    private void OnEnable()
    {
        EventBus.OnPlayerClickedOnGround += GoMove;
    }

    private void OnDisable()
    {
        EventBus.OnPlayerClickedOnGround -= GoMove;
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 3 , Color.blue, 0.1f);
        Vector3 LookVector = _destination - transform.position;
        
        
        transform.position = Vector3.SmoothDamp(transform.position, _destination, ref _velocity , SmoothTime,  Speed, Time.deltaTime);
            
        transform.forward = Vector3.RotateTowards(transform.forward, LookVector,10 * Mathf.Deg2Rad, 500);
        Quaternion rotation = transform.rotation;
        rotation.x = 0;
        transform.rotation = rotation;
        //transform.rotation = Quaternion.Euler(rotate);
            
        
        
        Debug.DrawRay(transform.position, LookVector , Color.yellow);
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down * 100, out hit))
        {
            Vector3 position = transform.position;
            position.y = hit.point.y + 0.5f;
            transform.position = position;
        }
    }

    public void GoMove(Vector3 point)
    {
        _destination = point;
        Debug.Log("Move");
    }
}
