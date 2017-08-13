using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	public GameObject exclamation;
	public float exclaimCleanupTime;
	public string playerTag;

	private bool spottedPlayer;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void DetectedPlayer(){
		// We don't want the exclamation to spam the view so make sure 
		// we don't spawn it if the player has already been spotted
		if(spottedPlayer == false)
			CreateExclamation();

		spottedPlayer = true;
	}

	private void CreateExclamation(){
		GameObject exclaim = Instantiate(exclamation, transform.position, exclamation.transform.localRotation);
		exclaim.transform.SetParent(transform);
		StartCoroutine(DestroyObject(exclaim, exclaimCleanupTime));
	}

	private IEnumerator DestroyObject(GameObject obj, float waitTime = 0f){
		yield return new WaitForSeconds(waitTime);
		Destroy(obj);
	}
}
