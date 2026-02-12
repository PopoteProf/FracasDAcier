using UnityEngine;

public class CubeMoveToward : MonoBehaviour
{
   public RayCastController RayCast;
   public float Speed = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, RayCast.Target, Speed * Time.deltaTime);
    }
}
