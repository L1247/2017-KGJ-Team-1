using UnityEngine;

public class PlanetGravity : MonoBehaviour
{
        //The gravity of the planet
        public float gravity = -10;
        Rigidbody rigi;
        private Transform m_transform;

        void Awake ()
        {
                m_transform = transform;
                rigi = GetComponent<Rigidbody>();
        }


        public void AddGravity ( Transform body )
        {
                //The gravity direction of the planet
                Vector3 gravityUp = ( body.position - m_transform.position ).normalized;
                Vector3 bodyUp = body.up;

                //Add the gravity to the target object
                body.GetComponent<Rigidbody>().AddForce( gravity * gravityUp );

                //Change the up direction of the target object to the reverse direction of gravity
                Vector3 targetUpDirection = body.up;
                Quaternion targetRotation = Quaternion.FromToRotation( bodyUp , gravityUp ) * body.rotation;
                body.rotation = Quaternion.Slerp( body.rotation , targetRotation , Time.deltaTime * 100 );
        }
}