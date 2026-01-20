using System;
using UnityEngine;

namespace Script
{
    public class SlimeContainer : Projectile
    {
        public override void SetUpProjectile(int damage, Vector3 force)
        {
            base.SetUpProjectile(damage, force);
        }

        private void Start()
        {
            _lastPos = transform.position;
        }

        protected override void Impact(RaycastHit hit)
        {
        }
    }
}
