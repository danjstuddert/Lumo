using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ViewArea))]
public class Beacon : MonoBehaviour {
	public string playerTag;
    private ViewArea viewArea;

    void Start() {
        viewArea = GetComponent<ViewArea>();
        viewArea.Init();
    }

	private void OnTriggerEnter(Collider other) {
		if (other.tag == playerTag)
			Debug.Log("Found the player!");
	}
}
