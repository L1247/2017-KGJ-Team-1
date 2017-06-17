using UnityEngine;

public class LogVelocity : MonoBehaviour
{
        Rigidbody rigi;
        // Use this for initialization
        void Start ()
        {
                rigi = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update ()
        {
                print( rigi.velocity );
        }
}
