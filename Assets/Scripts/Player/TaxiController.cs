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
        public float RayLength = 1 + .1f;
        [Header("倒數經過多久，計程車沒有撞到東西，就重製")]
        public float ResetTime;
        public LayerMask groundedMask;
        public string MotoName;
        public AudioClip Bububu;
        public bool CanPlayerControl;

        // System vars
        bool grounded;
        // 計程車可往右靠
        bool CanRightMove ;
        //可執行碰撞後面的事情
        bool CanPerformCollision;

        Vector3 moveAmount;
        Vector3 smoothMoveVelocity;
        Vector3 InitPostion;
        float verticalLookRotation;
        new Rigidbody rigidbody;
        Transform cameraTransform;
        Animator animator;


        #region Mono Events
        void Awake ()
        {
                //Cursor.lockState = CursorLockMode.Locked;
                //Cursor.visible = false;
                cameraTransform = Camera.main.transform;
                rigidbody = GetComponent<Rigidbody>();
                animator = GetComponent<Animator>();
                CanRightMove = true;
                CanPerformCollision = true;
                InitPostion = transform.position;
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

                //計程車靠右
                if ( Input.GetMouseButtonDown( 0 ) && CanRightMove )
                {
                        CanRightMove = false;
                        animator.SetBool( "Rotate" , true );
                        Invoke( "ResetGame" , ResetTime );
                }

        }

        void FixedUpdate ()
        {
                // Apply movement to rigidbody
                Vector3 localMove = transform.TransformDirection( moveAmount ) * Time.fixedDeltaTime;
                rigidbody.MovePosition( rigidbody.position + localMove );
        }

        /// <summary>
        /// 計程車碰撞到摩托車，產生特效，解開Joint
        /// </summary>
        /// <param name="collision"></param>
        public void OnCollisionEnter ( Collision collision )
        {
                if ( CanPerformCollision )
                {
                        if ( collision.collider.name.Equals( MotoName ) )
                        {
                                CanPerformCollision = false;
                                ccMessage.f_Broadcast( GameMessage.TaxiCollision );
                                collision.collider.GetComponent<MotoPlayer>().SetJointNull();
                                Instantiate( GameObjectFinder.GetObj( "FX_Hit_01" ) ,
                                 collision.transform.position - Vector3.forward * 4 + Vector3.up * 4 , Quaternion.identity );
                                CancelInvoke( "ResetGame" );
                        }
                }
        }
        #endregion

        /// <summary>
        /// 時間到，計程車沒有撞到東西，重置遊戲。
        /// 初始化位置，設為可右靠
        /// </summary>
        private void ResetGame ()
        {
                animator.SetBool( "Rotate" , false );
                transform.position = InitPostion;
                CanRightMove = true;
                CanPerformCollision = true;
                ccMessage.f_Broadcast( GameMessage.ResetGame );
        }
}
