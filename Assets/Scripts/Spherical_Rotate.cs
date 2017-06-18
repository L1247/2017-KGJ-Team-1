using UnityEngine;

public class Spherical_Rotate : MonoBehaviour
{
        public float RotateSpeed = 10f;
        // Use this for initialization
        void Start ()
        {

        }

        // Update is called once per frame
        void Update ()
        {
                transform.Rotate( Vector3.down * RotateSpeed * Time.deltaTime );
        }
}
