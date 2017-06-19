using UnityEngine;

public class AMaGameManager : Singleton<AMaGameManager>
{
        
        public float NPC_Spawn_Time = 5f;
        public float NPC_Spawn_Time_Random_AddMax = 2f;

        bool GameOver;
        GameObject NPC_ManPrefab;
        Transform NPC_GrandMother , NPC_Spawn_Postion , NPC_MAN_Parent;
        // Use this for initialization
        void Start ()
        {
                NPC_ManPrefab = GameObjectFinder.GetObj( "NPC_Man" );
                NPC_GrandMother = GameObjectFinder.GetObjTransform( "NPC_GrandMother" );
                NPC_Spawn_Postion = GameObjectFinder.GetObjTransform( "NPC_Spawn_Postion" );
                NPC_MAN_Parent = GameObjectFinder.GetObjTransform( "NPC_MAN_Parent" );
                Invoke( "SpawnNPC" , NPC_Spawn_Time );
        }

        // Update is called once per frame
        void Update ()
        {
                if ( GameOver )
                {
                        print( "GameOver" );
                        CancelInvoke(  );
                        this.enabled = false;
                }
        }

        void SpawnNPC ()
        {
                ccMessage.f_Broadcast( GameMessage.SpawnNPC );
                float RandomAddTime = Random.Range( 0 , NPC_Spawn_Time_Random_AddMax );
                float NextSpawnTime = NPC_Spawn_Time + RandomAddTime;
                Invoke( "SpawnNPC" , NextSpawnTime );
                GameObject NPC_Man_Instance = Instantiate( NPC_ManPrefab , NPC_ManPrefab.transform.position , NPC_Spawn_Postion.rotation );
                NPC_Man_Instance.transform.parent = NPC_MAN_Parent;
        }

        public void SetGameOver ()
        {
                GameOver = true;
        }
}
