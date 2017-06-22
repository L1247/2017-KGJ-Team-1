using UnityEngine;

public class NPC_Manager : MonoBehaviour
{
        public AudioClip HintAudio;
        // Use this for initialization
        void Start ()
        {
                if ( HintAudio )
                {
                        GameObjectFinder.GetObj( "Audio" ).GetComponent<AudioSource>().PlayOneShot( HintAudio );
                }
        }
}
