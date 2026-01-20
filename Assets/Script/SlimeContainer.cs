using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Script
{
    public class SlimeContainer : Projectile
    {
        public int NumberSlime;
        public GameObject SlimePrefab;
        
        public override void SetUpProjectile(int damage, Vector3 force)
        {
            base.SetUpProjectile(damage, force);
        }

        private void Start()
        {
            _lastPos = transform.position;
            for (int i = 0; i < NumberSlime; i++)
            {
                Vector3 randOffset = new Vector3(Random.Range(0.3f, -0.3f), Random.Range(0.3f, -0.3f), Random.Range(0.3f, -0.3f));
                Instantiate(SlimePrefab, randOffset + transform.position, Quaternion.identity, transform);
            }
        }

        protected override void Impact(RaycastHit hit)
        {
        }
    }
}
