using UnityEngine;

/// <summary>
/// 計程車控制器
/// </summary>
public class TaxiController : MonoBehaviour
{

        // public vars
        public float mouseSensitivityX = 1;
        public float mouseSensitivityY = 1;
        public float walkSpeed = 6;
        public float jumpForce = 220;
        public LayerMask groundedMask;
        public float RayLength = 1 + .1f;
        public string MotoName;
        public AudioClip Bububu;
        public bool CanPlayerControl;

        // System vars
        bool grounded;
        bool CanGen ;
        Vector3 moveAmount;
        Vector3 smoothMoveVelocity;
        float verticalLookRotation;
        Transform cameraTransform;
        new Rigidbody rigidbody;
        Animator animator;


        void Awake ()
        {
                //Cursor.lockState = CursorLockMode.Locked;
                //Cursor.visible = false;
                cameraTransform = Camera.main.transform;
                rigidbody = GetComponent<Rigidbody>();
                animator = GetComponent<Animator>();
                CanGen = true;
        }

        void Update ()
        {

                // Mouse Camera Look rotation:
                //transform.Rotate( Vector3.up * Input.GetAxis( "Mouse X" ) * mouseSensitivityX );
                //verticalLookRotation += Input.GetAxis( "Mouse Y" ) * mouseSensitivityY;
                //verticalLookRotation = Mathf.Clamp( verticalLookRotation , -60 , 60 );
                //cameraTransform.localEulerAngles = Vector3.left * verticalLookRotation;



                #region Self Control
                if ( CanPlayerControl )
                {

                        // Calculate movement:
                        float inputX = Input.GetAxisRaw( "Horizontal" );
                        float inputY = Input.GetAxisRaw( "Vertical" );

                        Vector3 moveDir = new Vector3( inputX , 0 , inputY ).normalized;
                        Vector3 targetMoveAmount = moveDir * walkSpeed;
                        moveAmount = Vector3.SmoothDamp( moveAmount , targetMoveAmount , ref smoothMoveVelocity , .15f );

                        // Jump
                        if ( Input.GetButtonDown( "Jump" ) )
                        {
                                if ( grounded )
                                {
                                        rigidbody.AddForce( transform.up * jumpForce );
                                }
                        }

                        // Grounded check
                        Ray ray = new Ray( transform.position + transform.up , -transform.up );
                        RaycastHit hit;

                        Debug.DrawRay( transform.position + transform.up , -transform.up , Color.red , RayLength );
                        if ( Physics.Raycast( ray , out hit , RayLength , groundedMask ) )
                        {
                                grounded = true;
                        }
                        else
                        {
                                grounded = false;
                        }
                }
                #endregion
                #region Auto Car Move
                else
                {
                        Vector3 moveDir = Vector3.forward.normalized;
                        Vector3 targetMoveAmount = moveDir * walkSpeed;
                        moveAmount = Vector3.SmoothDamp( moveAmount , targetMoveAmount , ref smoothMoveVelocity , .15f );
                }
                #endregion
                //Spawn Moto 
                if ( Input.GetMouseButtonDown( 0 ) && CanGen )
                {
                        CanGen = false;
                        animator.SetTrigger( "Rotate" );
                        Invoke( "GenMoto" , 1f );
                }

        }


        void FixedUpdate ()
        {
                // Apply movement to rigidbody
                Vector3 localMove = transform.TransformDirection( moveAmount ) * Time.fixedDeltaTime;
                rigidbody.MovePosition( rigidbody.position + localMove );
        }

        public void OnCollisionEnter ( Collision collision )
        {
                if ( collision.collider.name.Equals( MotoName ) )
                {
                        collision.collider.GetComponent<MotoPlayer>().SetJointNull();
                        Instantiate( GameObjectFinder.GetObj( "FX_Hit_01" ) ,
                         collision.transform.position - Vector3.forward  , Quaternion.identity );
                }
        }
        GameObject MotoCycleInstance;
        void GenMoto ()
        {
                GameObject MotoCyclePrefab = GameObjectFinder.GetObj( "MotorCycle" );
                Vector3 MotoGenPos = Vector3.right * 4.22f + Vector3.forward * transform.position.z;
                MotoCycleInstance = Instantiate( MotoCyclePrefab ,
                 MotoGenPos , Quaternion.identity );

                 Invoke( "ChangeCamera" , 2f );

        }

        void ChangeCamera ()
        {
                MotoCycleInstance.transform.Find( "NPC_GrandMother/Camera" ).gameObject.SetActive( true );
                Invoke( "ShowCutScene" , 1f );
        }

        void ShowCutScene ()
        {
                foreach ( var item in FindObjectsOfType<Camera>() )
                {
                        item.gameObject.SetActive( false );
                } 
                GameObjectFinder.GetObj( "CutScene" ).SetActive( true );
        }

}
