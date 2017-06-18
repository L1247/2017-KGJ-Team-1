using UnityEngine;

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
        new Rigidbody rigidbody;
        
        void Awake ()
        {
                rigidbody = GetComponent<Rigidbody>();
        }

        void Update ()
        {

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
                //string str = "";
                //if ( IsNotCollision )
                //        str = "Collision : ";
                //print( str+ rigidbody.velocity.magnitude );

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

        /// <summary>
        /// 阿罵 脫離Joint控制。
        /// </summary>
        public void SetJointNull ()
        {
                IsNotCollision = true;
                GetComponent<FixedJoint>().connectedBody = null;
                GameObjectFinder.GetObj( "NPC_GrandMother" ).GetComponent<Rigidbody>().useGravity = false;
                GameObjectFinder.GetObj( "Audio" ).GetComponent<AudioSource>().Play();
        }
}
