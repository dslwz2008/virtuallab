using UnityEngine;
using System.Collections;

public class PlayVideo : Photon.MonoBehaviour {

	public MovieTexture texVideo = null;
	
	// Use this for initialization
	void Start () {
		DistributeEvent.playVideo += OnMouseDown;
		renderer.material.mainTexture = texVideo;
		texVideo.Stop();
		texVideo.loop = true;
	}
	
	void OnMouseDown(){
		photonView.RPC("PlayOrPauseMovie", PhotonTargets.All);
	}
	
	[RPC]
	void PlayOrPauseMovie(PhotonMessageInfo info){
		if(texVideo.isPlaying){
			texVideo.Stop();
		}else{
			texVideo.Play();
		}
	}
}
