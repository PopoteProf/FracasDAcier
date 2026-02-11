using UnityEngine;

public class augmente : MonoBehaviour
{
    public bool isAugmenting = false;
    public float acceleration = 0.05f;
    public float maxSpeed = 5f;
    public float minSpeed = 0f;

    public float speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isAugmenting && speed < maxSpeed)
        {
            speed += acceleration;
        }

        if (!isAugmenting && speed > minSpeed)
        {
            speed -= acceleration;
        }
    }
}
