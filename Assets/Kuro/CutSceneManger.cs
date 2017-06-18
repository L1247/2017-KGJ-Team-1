using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CutSceneManger : MonoBehaviour {
	
	public List< GameObject> Characters;
	public int CharacterNumber;
	public Transform CharacterGroup;
	public float RotateSpeed;
	public List< Transform >PlanetList;
	public int NowPlanet;
	public GameObject Explosion;
	public AudioSource ExplosionSound;
	public Transform ExplosionSpawnPoint;
	public GameObject SpeedEffect;
	// Use this for initialization
	void Start () {
		Characters [CharacterNumber].SetActive (true);
	
		StartCoroutine (PlanetChange());


	}
	
	// Update is called once per frame
	void Update () {
		CharacterGroup.eulerAngles += Vector3.forward*Time.deltaTime*RotateSpeed;
	}


	IEnumerator PlanetChange(){
		
		PlanetList [NowPlanet].DOLocalMoveZ (3f, 2).SetEase (Ease.OutBounce);

		yield return new WaitForSeconds(2f);


		if (NowPlanet != PlanetList.Count - 1) {
			PlanetList [NowPlanet].DOLocalMoveX (3, 1).SetEase (Ease.InOutExpo);

			NowPlanet++;
			StartCoroutine (PlanetChange ());

		} else {
		
			CharacterGroup.DOScale (Vector3.zero, 0.5f).OnComplete(CharacterGone);
			SpeedEffect.SetActive (false);
		}

	}


	void CharacterGone(){
		
		ExplosionSound.Play ();
		Instantiate (Explosion,ExplosionSpawnPoint.position,Quaternion.identity);
	
	}

}
