using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Generator.
/// version : 0.5
/// </summary>
public class Generator : MonoBehaviour {
	/// <summary>
	/// The charactors. user specified prefabs.
	/// </summary>
	public List<GameObject> charactors;
	
	/// <summary>
	/// The last frame data
	/// </summary>
	Frame lastFrame = null;
	
	GameObject parent = null;
	
	/// <summary>
	/// The time of one frame.
	/// </summary>
	public float frameTime = 0.5f;
	
	// Use this for initialization
	void Start () {
		parent = GameObject.Find("/Instances");
		InvokeRepeating("GenerateOneFrame", 0.5f, frameTime);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void GenerateOneFrame(){
		Queue<Frame> queue = FrameBuffer.GetInstance().GetQueue();
		//no data here
		if(queue.Count == 0){
			//every body idle
			foreach(Transform people in parent.transform){
				foreach(Transform t in people.transform){
					if(t.gameObject.animation){
						t.gameObject.animation.wrapMode = WrapMode.Loop;
						t.gameObject.animation.Play("idle");
						break;
					}
				}
			}
			CancelInvoke("GenerateOneFrame");
			Debug.Log("Simulation Finished!");
			return;
		}
		
		Frame thisFrame = queue.Dequeue();
		
		List<int> newIDs = new List<int>();
		List<int> delIDs = new List<int>();
		List<int> sameIDs = new List<int>();
		
		//this is the first frame
		if(lastFrame == null){
			newIDs = new List<int>(thisFrame.peoples.Keys);
		}else{
			Dictionary<string, List<int>> dict = 
				Utilities.CompareTwoFrames(lastFrame, thisFrame);
			newIDs = dict["new"];
			delIDs = dict["del"];
			sameIDs = dict["same"];
		}
		
		System.Random random = new System.Random();
		if(newIDs.Count != 0){
			foreach(int id in newIDs){
				int chrctIndex = random.Next(charactors.Count);
				GameObject instance = (GameObject)Instantiate(charactors[chrctIndex],
					thisFrame.peoples[id].m_position, Quaternion.identity);
				//LODGroup Object
				instance.name = "people" + id.ToString();
				instance.transform.parent = parent.transform;
				foreach(Transform t in instance.transform){
					if(t.gameObject.animation){
						t.gameObject.animation.wrapMode = WrapMode.Loop;
						t.gameObject.animation.Play("walk");
						break;
					}
				}
			}
		}
		if(delIDs.Count != 0){
			foreach(int id in delIDs){
				string name = "/Instances/people" + id.ToString();
				Destroy(GameObject.Find(name));
			}
		}
		if(sameIDs.Count != 0){
			foreach(int id in sameIDs){
				string name = "/Instances/people" + id.ToString();
				People p = thisFrame.peoples[id];
				Vector3 target = p.m_position;
				int status = p.m_status;
				GameObject people = GameObject.Find(name);
				if(status == 0){//walk
					foreach(Transform t in people.transform){
						if(t.gameObject.animation){
							t.gameObject.animation.wrapMode = WrapMode.Loop;
							t.gameObject.animation.Play("walk");
							break;
						}
					}
					//move
					iTween.MoveTo(people, 
						iTween.Hash("position",target,
						"time",frameTime,
						"orienttopath",true,
						"looptype",iTween.LoopType.none,
						"easetype",iTween.EaseType.linear));
				}else if(status == 1){//idle
					foreach(Transform t in people.transform){
						if(t.gameObject.animation){
							t.gameObject.animation.wrapMode = WrapMode.Loop;
							t.gameObject.animation.Play("idle");
							break;
						}
					}
					//move
					iTween.MoveTo(people, 
						iTween.Hash("position",target,
						"time",frameTime,
						"orienttopath",false,
						"looptype",iTween.LoopType.none,
						"easetype",iTween.EaseType.linear));
				}
				
			}
		}
		lastFrame = thisFrame;
	}
}
