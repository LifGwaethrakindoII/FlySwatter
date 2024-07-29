using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace Voidless.XRIT
{
    public class XRITFlySwatter : MonoBehaviour
    {
        [SerializeField] private XRGrabInteractable _grabInteractable;
        [SerializeField] private TrailRenderer _swingRenderer;
        [SerializeField] private float _minVelocityMagnitude;
        [SerializeField] private float _maxVelocityMagnitude;
        [SerializeField] private float _maxSwingForce;
        private Vector3 _previousInteractorPosition;
        private Vector3 _interactorVelocity;
        private Vector3 _backFacePoint;
        private Vector3 _frontFacePoint;
        private Rigidbody _rigidbody;

        /// <summary>Gets grabInteractable property.</summary>
        public XRGrabInteractable grabInteractable { get { return _grabInteractable; } }

        public TrailRenderer swingRenderer { get { return _swingRenderer; } }

        public float minVelocityMagnitude { get { return _minVelocityMagnitude; } }

        public float maxVelocityMagnitude { get { return _maxVelocityMagnitude; } }

        public float maxSwingForce { get { return _maxSwingForce; } }

        /// <summary>Gets and Sets previousInteractorPosition property.</summary>
        public Vector3 previousInteractorPosition
        {
            get { return _previousInteractorPosition; }
            set { _previousInteractorPosition = value; }
        }

        /// <summary>Gets and Sets interactorVelocity property.</summary>
        public Vector3 interactorVelocity
        {
            get { return _interactorVelocity; }
            set { _interactorVelocity = value; }
        }

        /// <summary>Gets and Sets backFacePoint property.</summary>
        public Vector3 backFacePoint
        {
            get { return _backFacePoint; }
            set { _backFacePoint = value; }
        }

        /// <summary>Gets and Sets frontFacePoint property.</summary>
        public Vector3 frontFacePoint
        {
            get { return _frontFacePoint; }
            set { _frontFacePoint = value; }
        }

        public Rigidbody rigidbody
        {
            get { return _rigidbody; }
            set { _rigidbody = value; }
        }

        private void Awake()
        {
            if(grabInteractable == null) return;

            swingRenderer.gameObject.SetActive(false);
            grabInteractable.selectEntered.AddListener(OnSelectEnter);
            grabInteractable.selectExited.AddListener(OnSelectExit);
            if(!grabInteractable.TryGetComponent<Rigidbody>(out _rigidbody)) TryGetComponent<Rigidbody>(out _rigidbody);
        }

        private void OnSelectEnter(SelectEnterEventArgs a)
        {
            swingRenderer.gameObject.SetActive(true);
        }

        private void OnSelectExit(SelectExitEventArgs a)
        {
            swingRenderer.gameObject.SetActive(false);
        }

        /// <summary>Updates XRFlySwatter's instance at each frame.</summary>
        private void Update()
        {
            Debug.Log("Velocity: " + GetVelocity() + ", Angular Velocity: " + GetAngularVelocity());
        }

        /// <summary>Event triggered when this Collider/Rigidbody begun having contact with another Collider/Rigidbody.</summary>
        /// <param name="col">The Collision data associated with this collision Event.</param>
        private void OnCollisionEnter(Collision col)
        {
            GameObject obj = col.gameObject;
    
            Debug.Log("[XRFlySwatter] Entered collision with " + obj.name + " with a velocity magnitude of: " + GetVelocitySqrMagnitude() + " and a force of: " + GetCurrentSwingForce());
        }

        public Vector3 GetVelocity()
        {
            return rigidbody != null ? rigidbody.velocity : Vector3.zero;
        }

        public Vector3 GetAngularVelocity()
        {
            return rigidbody != null ? rigidbody.angularVelocity: Vector3.zero;
        }

        public float GetVelocitySqrMagnitude()
        {
            return GetVelocity().sqrMagnitude;
        }

        public float GetCurrentSwingForce()
        {
            float minSqr = minVelocityMagnitude * minVelocityMagnitude;
            float maxSqr = maxVelocityMagnitude * maxVelocityMagnitude;
            float x = Mathf.Min(GetVelocitySqrMagnitude(), maxSqr);
            float t = RemapValueToNormalizedRange(x, minSqr, maxSqr);

            return t * maxSwingForce;
        }

        /// <summary>Remaps given input from map into normalized range.</summary>
        /// <param name="_input">Input value to remap.</param>
        /// <param name="_map">Original values mapping.</param>
        /// <returns>Input mapped into normalizedRange.</returns>
        public static float RemapValueToNormalizedRange(float _input, float _mapMin, float _mapMax)
        {
            return ((_input - _mapMin) / (_mapMax - _mapMin));
        }
    }
}