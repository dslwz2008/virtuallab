using UnityEngine;
using System.Collections;

public class DataSupervisor : MonoBehaviour {
	
	private ReadAndParseDataFromDB rap = null;
	
	// Use this for initialization
	void Start () {
		rap = gameObject.GetComponent<ReadAndParseDataFromDB>();
	}
	
	// Update is called once per frame
	void Update () {
		int curCount = FrameBuffer.GetInstance().GetQueue().Count;
		if(curCount < 200){//speed up the read frequency
			if(rap.readInterval > 5){
				Debug.Log("must speed up");
				rap.readInterval = 5;
				rap.ReInitialiseReader();
			}
		}else if(curCount > 500){//slow down the read frequency
			if(rap.readInterval < 20){
				Debug.Log("must slow down");
				rap.readInterval = 20;
				rap.ReInitialiseReader();
			}
		}
	}
}
