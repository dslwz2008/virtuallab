using UnityEngine;
using System.Collections;

public class ListenNetwork : MonoBehaviour {

	void OnEnable(){
		//StartCoroutine(NetworkUtils.ListenAndHandle());
	}
	
	void OnDisable(){
		StopAllCoroutines();
	}
	
	
}
