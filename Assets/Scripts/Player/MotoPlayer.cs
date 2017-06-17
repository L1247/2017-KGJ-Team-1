using UnityEngine;

[RequireComponent( typeof( GravityBody ) )]
public class MotoPlayer : MonoBehaviour
{

        // public vars
        public float walkSpeed = 6;
        public LayerMask groundedMask;
        public float RayLength = 1 + .1f;

        // System vars
        bool grounded;
        bool IsNotCollision;
        Vector3 moveAmount;
        Vector3 smoothMoveVelocity;
        float verticalLookRotation;
        Transform cameraTransform;
        Rigidbody rigidbody;


        void Awake ()
        {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                cameraTransform = Camera.main.transform;
                rigidbody = GetComponent<Rigidbody>();
        }

        void Update ()
        {
                
                // Look rotation:
                verticalLookRotation = Mathf.Clamp( verticalLookRotation , -60 , 60 );

                // Calculate movement:
                float inputX = Input.GetAxisRaw( "Horizontal" );
                float inputY = Input.GetAxisRaw( "Vertical" );

                Vector3 moveDir = new Vector3( inputX , 0 , 20 ).normalized;
                Vector3 targetMoveAmount = moveDir * walkSpeed;
                moveAmount = Vector3.SmoothDamp( moveAmount , targetMoveAmount , ref smoothMoveVelocity , .15f );

                // Grounded check
                Ray ray = new Ray( transform.position , -transform.up );
                RaycastHit hit;

                Debug.DrawRay( transform.position , -transform.up , Color.red , RayLength );

                if ( Physics.Raycast( ray , out hit , RayLength , groundedMask ) )
                {
                        grounded = true;
                }
                else
                {
                        grounded = false;
                }

        }

        void FixedUpdate ()
        {
                if ( IsNotCollision )
                {
                        return;
                }
                // Apply movement to rigidbody
                Vector3 localMove = transform.TransformDirection( moveAmount ) * Time.fixedDeltaTime;
                rigidbody.MovePosition( rigidbody.position + localMove );
        }

        public void SetJointNull ()
        {
                IsNotCollision = true;
                GetComponent<FixedJoint>().connectedBody = null;
                GameObjectFinder.GetObj( "AMa" ).GetComponent<Rigidbody>().useGravity = false;
        }
}
