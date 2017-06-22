using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 阿罵場景管理器
/// </summary>
public class AMaGameManager : Singleton<AMaGameManager>
{
        [Header("每隔多久的產生時間")]
        public float NPC_Spawn_Time = 5f;
        [Header("隨機產生時間，與Spawn_Time相加")]
        public float NPC_Spawn_Time_Random_AddMax = 2f;
        [Header("路人NPC刪除時間")]
        public float NPC_Destroy_Time;
        [Header("路人Prefabs")]
        public List<GameObject> NPC_Prefabs;


        Transform NPC_GrandMother , NPC_Spawn_Postion , NPC_MAN_Parent ,Moto_Spawn_Parent;

        #region Mono Events
        // Use this for initialization
        void Start ()
        {
                NPC_GrandMother = GameObjectFinder.GetObjTransform( "NPC_GrandMother" );
                NPC_Spawn_Postion = GameObjectFinder.GetObjTransform( "NPC_Spawn_Postion" );
                NPC_MAN_Parent = GameObjectFinder.GetObjTransform( "NPC_MAN_Parent" );
                Moto_Spawn_Parent = GameObjectFinder.GetObjTransform( "Moto_Spawn_Parent" );
                ccMessage.f_AddListener( GameMessage.TaxiCollision , TaxiCollision );
                ccMessage.f_AddListener( GameMessage.ResetGame , ResetGame );
                Invoke( "SpawnNPC" , NPC_Spawn_Time );
        }



        // Update is called once per frame
        //void Update ()
        //{

        //}
        #endregion

        #region 產生路人，並招手，時間到自動刪除路人
        void SpawnNPC ()
        {
                SpawnMoto();
                float RandomAddTime = Random.Range( 0 , NPC_Spawn_Time_Random_AddMax );
                float NextSpawnTime = NPC_Spawn_Time + RandomAddTime;
                Invoke( "SpawnNPC" , NextSpawnTime );
                int randomIndex = Random.Range( 0 , NPC_Prefabs.Count );
                GameObject NPC_Prefab = NPC_Prefabs[ randomIndex ];
                GameObject NPC_Instance = Instantiate( NPC_Prefab , NPC_Prefab.transform.position , NPC_Spawn_Postion.rotation );
                NPC_Instance.transform.parent = NPC_MAN_Parent;
                Destroy( NPC_Instance , NPC_Destroy_Time );
        }
        #endregion

        #region 摩托車
        GameObject MotoCycleInstance;
        void SpawnMoto ()
        {
                GameObject MotoCyclePrefab = GameObjectFinder.GetObj( "MotorCycle" );
                Vector3 MotoGenPos = /*Vector3.right * 4.22f + Vector3.back * 10 + Vector3.down * 4*/Moto_Spawn_Parent.position;
                //Debug.LogError( MotoGenPos );
                MotoCycleInstance = Instantiate( MotoCyclePrefab ,
                 MotoGenPos , Quaternion.identity );
                MotoCycleInstance.transform.parent = Moto_Spawn_Parent.transform;
        }

        /// <summary>
        /// 切換鏡頭到摩托車阿罵身上
        /// </summary>
        void ChangeCamera ()
        {
                MotoCycleInstance.transform.Find( "NPC_GrandMother/Camera" ).gameObject.SetActive( true );
                Invoke( "ShowCutScene" , 1f );
        }

        /// <summary>
        /// Show 結束動畫
        /// </summary>
        void ShowCutScene ()
        {
                foreach ( var item in FindObjectsOfType<Camera>() )
                {
                        if ( item.transform.parent.name.Contains( "CutScene" ) == false )
                                item.gameObject.SetActive( false );
                }
                GameObjectFinder.GetObj( "CutScene" ).SetActive( true );
        }
        #endregion

        /// <summary>
        /// 收到計程車撞擊事件，停止產生，時間到切換攝影機
        /// </summary>
        /// <param name="o_data"></param>
        public void TaxiCollision ( object o_data )
        {
                Invoke( "ChangeCamera" , 2f );
                CancelInvoke( "SpawnNPC" );
        }

        private void ResetGame ( object data )
        {
                for ( int i = 0 ; i < Moto_Spawn_Parent.transform.childCount ; i++ )
                {
                        Destroy( Moto_Spawn_Parent.transform.GetChild( i ).gameObject );
                }
        }

}
