using UnityEngine;
using System.Collections;

public class AnimateTexture : Photon.MonoBehaviour {
	
	public float repeatRate = 0.3f;
	private int index = 0;
	private bool hasStart = false;
	
	// Use this for initialization
	void Start () {
		DistributeEvent de = GameObject.Find("/Scripts").GetComponent<DistributeEvent>();
		de.animateTexture += StartAnimation;
	}
	
	void StartAnimation(){
		if(!hasStart){
			InvokeRepeating("DisplayTexturesFromBuffer", 0.0f, repeatRate);
			hasStart = true;
		}
	}
	
	void DisplayTexturesFromBuffer(){
		photonView.RPC("ChangeQuadTexture", PhotonTargets.AllBuffered);
	}
	
	[RPC]
	void ChangeQuadTexture(PhotonMessageInfo info){
		renderer.material.mainTexture = NetworkUtils.textures[index];
		index++;
		index %= (NetworkUtils.textures.Count);
	}
}
