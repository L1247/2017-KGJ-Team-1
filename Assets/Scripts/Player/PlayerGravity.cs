using UnityEngine;

public class PlayerGravity : MonoBehaviour
{
        public PlanetGravity planetGravity;

        private Transform m_transform;
        Rigidbody rigibody;

        void Awake ()
        {
                rigibody = GetComponent<Rigidbody>();
                m_transform = transform;
        }

        void Start ()
        {
                rigibody.constraints = RigidbodyConstraints.FreezeRotation;
                rigibody.useGravity = false;
        }


        void Update ()
        {
                planetGravity.AddGravity( m_transform );
        }
}