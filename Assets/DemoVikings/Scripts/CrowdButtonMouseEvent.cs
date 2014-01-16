using UnityEngine;
using System.Collections;

public class CrowdButtonMouseEvent : Photon.MonoBehaviour {
	
	private Shader diffuse = null;
	private Shader selfillumin = null;
	private ReadAndParseDataFromDB rapd = null;
	private Generator gen = null;
	private DataSupervisor ds = null;
	private bool hasBegin = false;
	
	// Use this for initialization
	void Start () {
		diffuse = Shader.Find("Diffuse");
		selfillumin = Shader.Find("Self-Illumin/Diffuse");
		
		GameObject goScripts = GameObject.Find("/Scripts");
		rapd = goScripts.GetComponent<ReadAndParseDataFromDB>();
		gen = goScripts.GetComponent<Generator>();
		ds = goScripts.GetComponent<DataSupervisor>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnMouseEnter(){
		renderer.material.shader = selfillumin;
	}
	
	void OnMouseExit(){
		renderer.material.shader = diffuse;
	}
	
	void OnMouseDown(){
		photonView.RPC("TurnOnOrOffCrowd", PhotonTargets.All);
	}
	
	[RPC]
	void TurnOnOrOffCrowd(){
		if(hasBegin){//turn off
			rapd.enabled = false;
			gen.enabled = false;
			ds.enabled = false;
			FrameBuffer.GetInstance().Clear();
		}else{//turn on
			rapd.enabled = true;
			gen.enabled = true;
			ds.enabled = true;
		}
		hasBegin = !hasBegin;
	}
}
