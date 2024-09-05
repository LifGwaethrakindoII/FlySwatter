using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless.FlySwatter
{
    public enum Gender { Male, Female, NonBinary } // Joke, animals different than humans don't delude themselves.

    [RequireComponent(typeof(SteeringVehicle))]
    public class Enemy : PoolGameObject, IStateMachine
    {
        public const float SQRDISTANCE_DESTINY = 0.2f;

        [Space(5f)]
        [SerializeField] private LayerMask _surfaceMask;
        [Space(5f)]
        [Header("Thresholds:")]
        [SerializeField] private float _healthThreshold;
        [SerializeField] private float _hungerThreshold;
        [SerializeField] private float _ageThreshold;
        [SerializeField] private float _maxHungerForReproduction;
        [Space(5f)]
        [Header("Increase/Decrease Rates:")]
        [SerializeField] private float _hungerIncreaseRate;
        [SerializeField] private float _healthDecreaseRate;
        [SerializeField] private float _flyingConsumptionRate;
        [SerializeField] private float _hungerWhileRestingDecreaseRate;
        [Space(5f)]
        [Header("Time Periods:")]
        [SerializeField] private FloatRange _restingTimeRange;
        [SerializeField] private FloatRange _wanderInterval;
        private SteeringVehicle _vehicle;
        private float _health;
        private int _state;
        private int _previousState;
        private float _hunger;
        private float _age;
        private Gender _gender;
        protected Coroutine currentRoutine;

        /// <summary>Gets surfaceMask property.</summary>
        public LayerMask surfaceMask { get { return _surfaceMask; } }

        /// <summary>Gets restingTimeRange property.</summary>
        public FloatRange restingTimeRange { get { return _restingTimeRange; } }

        /// <summary>Gets and Sets gender property.</summary>
        public Gender gender
        {
            get { return _gender; }
            set { _gender = value; }
        }

        /// <summary>Gets healthThreshold property.</summary>
        public float healthThreshold { get { return _healthThreshold; } }

        /// <summary>Gets hungerThreshold property.</summary>
        public float hungerThreshold { get { return _hungerThreshold; } }

        /// <summary>Gets ageThreshold property.</summary>
        public float ageThreshold { get { return _ageThreshold; } }

        /// <summary>Gets maxHungerForReproduction property.</summary>
        public float maxHungerForReproduction { get { return _maxHungerForReproduction; } }

        /// <summary>Gets hungerIncreaseRate property.</summary>
        public float hungerIncreaseRate { get { return _hungerIncreaseRate; } }

        /// <summary>Gets healthDecreaseRate property.</summary>
        public float healthDecreaseRate { get { return _healthDecreaseRate; } }

        /// <summary>Gets flyingConsumptionRate property.</summary>
        public float flyingConsumptionRate { get { return _flyingConsumptionRate; } }

        /// <summary>Gets hungerWhileRestingDecreaseRate property.</summary>
        public float hungerWhileRestingDecreaseRate { get { return _hungerWhileRestingDecreaseRate; } }

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

        /// <summary>Gets and Sets age property.</summary>
        public float age
        {
            get { return _age; }
            set { _age = value; }
        }

        /// <summary>Gets and Sets health property.</summary>
        public float health
        {
            get { return _health; }
            set { _health = value; }
        }

        public float hunger
        {
            get { return _hunger; }
            set { _hunger = value; }
        }

        public int ignoreResetMask
        {
            get { return 0; }
            set {  }
        }

#region IStateMachine:
        public void OnEnterState(int _state)
        {
            
        }

        public void OnExitState(int _state)
        {
            
        }

        public void OnStatesAdded(int _state)
        {
            
        }

        public void OnStatesRemoved(int _state)
        {
            
        }
#endregion

        /// <summary>Updates Enemy's instance at the end of each frame.</summary>
        protected override void LateUpdate()
        {
            base.LateUpdate();

            UpdateStats();
            EvaluateStates();
        }

        /// <summary>Updates stats.</summary>
        protected void UpdateStats()
        {
            bool flying = (state | FlySwatterGameData.STATE_FLAG_FLYING) == state;
            float dt = Time.deltaTime;
            float additionalRate = flying ? flyingConsumptionRate : 0.0f;
            float hungerRate = flying ? 1.0f : hungerWhileRestingDecreaseRate;

            hunger += ((hungerIncreaseRate + additionalRate) * hungerRate * dt);
            health -= (healthDecreaseRate * (1.0f + (hunger / hungerThreshold) * dt));
            age += dt;
        }

        /// <summary>Evaluates stats.</summary>
        protected virtual void EvaluateStates()
        {
            if(health <= 0.0f)
            {
                // Die
                return;
            }

            if(age >= ageThreshold)
            {
                if(hunger >= maxHungerForReproduction)
                {
                    // Set reproduction as priority
                }
            }

            if(hunger >= hungerThreshold)
            {
                // Seek food is a priority.
            }

            else
            {
                if((state | FlySwatterGameData.STATE_FLAG_WANDERING) != state)
                BeginWanderRoutine();
            }
        }

        protected virtual void BeginWanderRoutine()
        {
            state &= ~FlySwatterGameData.STATE_FLAG_HUNGRY;
            state |= FlySwatterGameData.STATE_FLAG_WANDERING;

            this.StartCoroutine(WanderBehavior(), ref currentRoutine);
        }

        /// <summary>Begins Resting's Routine.</summary>
        protected virtual void BeginRestRoutine()
        {
            state &= ~FlySwatterGameData.STATE_FLAG_HUNGRY;
            state |= FlySwatterGameData.STATE_FLAG_RESTING;

            this.StartCoroutine(RestRoutine(), ref currentRoutine);
        }

        protected virtual IEnumerator WanderBehavior()
        {
            yield return null;
        }

        /// <summary>Resting's Routine.</summary>
        protected virtual IEnumerator RestRoutine()
        {
            float restTime = restingTimeRange.Random();
            float time = 0.0f;

            while(time < restTime)
            {
                time += Time.deltaTime;
                yield return null;
            }

            state &= ~FlySwatterGameData.STATE_FLAG_RESTING;
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

        /// <returns>String representing this Enemy.</returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("Gender: ");
            builder.AppendLine(gender.ToString());
            builder.Append("Health: ");
            builder.AppendLine(health.ToString());
            builder.Append("Hunger: ");
            builder.Append(hunger.ToString());
            builder.Append("Age: ");
            builder.Append(age.ToString());
            builder.Append("States: ");
            builder.AppendLine(VString.GetNamedBitChain(state, FlySwatterGameData.FLAGNAMES_ENEMY));

            return builder.ToString();
        }
    }
}