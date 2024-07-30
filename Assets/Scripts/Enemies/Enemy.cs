using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless.FlySwatter
{
    [RequireComponent(typeof(SteeringVehicle))]
    public class Enemy : PoolGameObject, IStateMachine
    {
        public const float SQRDISTANCE_DESTINY = 0.2f;

        private SteeringVehicle _vehicle;
        private int _state;
        private int _previousState;

        public SteeringVehicle vehicle
        {
            get
            {
                if (_vehicle == null) _vehicle = GetComponent<SteeringVehicle>();
                return _vehicle;
            }
        }

        public int state
        {
            get { return _state; }
            set { _state = value; }
        }

        public int previousState
        {
            get { return _previousState; }
            set { _previousState = value; }
        }

        public int ignoreResetMask { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public void OnEnterState(int _state)
        {
            throw new System.NotImplementedException();
        }

        public void OnExitState(int _state)
        {
            throw new System.NotImplementedException();
        }

        public void OnStatesAdded(int _state)
        {
            throw new System.NotImplementedException();
        }

        public void OnStatesRemoved(int _state)
        {
            throw new System.NotImplementedException();
        }

        protected virtual IEnumerator GoToDestinationRoutine(Vector3 _destiny)
        {
            Vector3 direction = _destiny - transform.position;

            while (direction.sqrMagnitude > SQRDISTANCE_DESTINY)
            {
                Vector3 steeringForce = vehicle.GetSeekForce(_destiny);
                
                vehicle.ApplyForce(steeringForce);
                vehicle.Displace(Time.fixedDeltaTime);

                yield return VCoroutines.WAIT_PHYSICS_THREAD;

                direction = _destiny - transform.position;
            }
        }
    }
}