using UnityEngine;
using System.Collections;

public class TriggerPlayVideo : MonoBehaviour {
	void OnMouseDown(){
		DistributeEvent de = GameObject.Find("/Scripts").GetComponent<DistributeEvent>();
		de.playVideo();
	}
}
