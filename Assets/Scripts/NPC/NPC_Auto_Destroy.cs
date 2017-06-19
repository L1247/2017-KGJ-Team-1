using UnityEngine;

public class NPC_Auto_Destroy : MonoBehaviour {
        public float DestroyTime;
	// Use this for initialization
	void Start () {
                Destroy( gameObject , DestroyTime );
	}
}
