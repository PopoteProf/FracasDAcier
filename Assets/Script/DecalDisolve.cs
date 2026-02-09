using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using Random = UnityEngine.Random;

public class DecalDisolve : MonoBehaviour
{
    private DecalProjector _decalProjector;
    private bool _disolve;
    public float _disolveSpeed;
    
    private void Start()
    {
        _decalProjector = GetComponent<DecalProjector>();
        float rand = Random.Range(2f, 5f);
        _disolveSpeed = Random.Range(0.1f, 0.8f);
        StartCoroutine("Delay", rand);
    }

    public IEnumerator Delay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("TRUE");
        _disolve = true;
    }

    private void Update()
    {
        if (_disolve)
        {
            _decalProjector.fadeFactor -= _disolveSpeed * Time.deltaTime;
            if (_decalProjector.fadeFactor <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }
}
