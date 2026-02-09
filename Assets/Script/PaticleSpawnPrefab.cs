using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Script
{
    public class PaticleSpawnPrefab : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _ps;
        public GameObject Decal;
        public Vector2 RandSize;
        
        private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

        public void OnParticleCollision(GameObject other)
        {
            int num = _ps.GetCollisionEvents(other, collisionEvents);

            for (int i = 0; i < num; i++)
            {
                Vector3 pos = collisionEvents[i].intersection;
                Vector3 normal = collisionEvents[i].normal;
                Quaternion rot = Quaternion.LookRotation(normal);

                DecalProjector decalProjector = Instantiate(Decal, pos, rot).GetComponent<DecalProjector>();
                float randSize = Random.Range(RandSize.x, RandSize.y);
                decalProjector.size = new Vector3(randSize,randSize, randSize);
                
                Debug.Log("Spawn Decal");
            }
        
        }
    }
}
