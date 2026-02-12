using UnityEngine;

public class FollowCube : MonoBehaviour
{
    
    public Transform Target;
    public Vector3 CurrentVelocity = Vector3.zero;
    public float SmoothTime = 0.3f;
    public float HeightOffset = 0.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, Target.position, ref CurrentVelocity, SmoothTime,Mathf.Infinity, Time.deltaTime);
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit))
        {
            transform.position =  new Vector3(transform.position.x, hit.point.y + HeightOffset, transform.position.z);
        }
        transform.forward = Vector3.RotateTowards(transform.forward, Target.position - transform.position, 5f * Time.deltaTime, 0.0f);
    }
}