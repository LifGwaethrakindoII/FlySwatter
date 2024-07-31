using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless.FlySwatter
{
    public class FlyEnemy : Enemy
    {
        [SerializeField] private Bounds traversableBounds;
        private Vector3 currentTarget;

        /// <summary>Draws Gizmos on Editor mode.</summary>
        private void OnDrawGizmos()
        {
            VGizmos.DrawBounds(traversableBounds);
            Gizmos.DrawSphere(currentTarget, 0.1f);
        }

        protected override void Awake()
        {
            base.Awake();
            this.StartCoroutine(Behavior(), ref currentRoutine);
        }

        public override void OnObjectRecycled()
        {
            base.OnObjectRecycled();
            this.StartCoroutine(Behavior(), ref currentRoutine);
        }

        private IEnumerator Behavior()
        {
            float tolerance = 5.0f;
            float t = 0.0f;
            currentTarget = traversableBounds.GetRandomPoint();

            while(true)
            {
                Vector3 direction = currentTarget - transform.position;
                Vector3 seekForce = vehicle.GetSeekForce(currentTarget);
                float dt = Time.deltaTime;

                vehicle.ApplyForce(seekForce);
                vehicle.Displace(dt);
                vehicle.Rotate(dt);

                if(direction.sqrMagnitude <= SQRDISTANCE_DESTINY || t >= tolerance)
                {
                    t = 0.0f;
                    currentTarget = traversableBounds.GetRandomPoint();
                }
                else t += dt;

                yield return null;
            }
        }
    }
}