using UnityEngine;
using System.Collections;

public class ChangeTexture : Photon.MonoBehaviour {
	
	public Texture[] textures;
	private int index = 0;
	
	// Use this for initialization
	void Start () {
		DistributeEvent.changeTexture += OnMouseDown;
		renderer.material.mainTexture = textures[index];
	}
	
	void OnMouseDown(){
		photonView.RPC("ChangeQuadTexture", PhotonTargets.AllBuffered);
	}
	
	[RPC]
	void ChangeQuadTexture(PhotonMessageInfo info){
		index = ++index % textures.Length;
		renderer.material.mainTexture = textures[index];
	}
}
