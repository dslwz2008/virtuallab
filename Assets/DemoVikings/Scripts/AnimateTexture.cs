using UnityEngine;
using System.Collections;

public class AnimateTexture : Photon.MonoBehaviour {
	
	public float repeatRate = 0.3f;
	private int index = 0;
	
	// Use this for initialization
	void Start () {
		DistributeEvent.animateTexture += StartAnimation;
	}
	
	void StartAnimation(){
		InvokeRepeating("DisplayTexturesFromBuffer", 0.0f, repeatRate);
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
