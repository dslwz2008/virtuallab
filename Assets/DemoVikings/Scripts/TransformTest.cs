using UnityEngine;
using System.Collections;

public class TransformTest : MonoBehaviour {
	public GameObject source = null;
	public GameObject target = null;

	// Use this for initialization
	void Start () {
//		source.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
//		source.transform.Rotate(0.0f, 180.0f, 0.0f);
		Vector3 offset = new Vector3(source.transform.position.x - target.transform.position.x, 
			target.transform.position.y,
			source.transform.position.z - target.transform.position.z);
//		source.transform.Translate(offset);
		Matrix4x4 m = Matrix4x4.TRS(Vector3.zero,//offset,
			Quaternion.identity,//Euler(0.0f, 180.0f, 0.0f),
			new Vector3(0.05f, 0.05f, 0.05f)
			);
		//source.transform.position = m.MultiplyPoint(source.transform.position);
		foreach(Transform t in source.transform){
			t.transform.position = PreTransform(t.transform.position);
		}
	}
	
	Vector3 PreTransform(Vector3 pos){
		Vector3 newpos = (pos - source.transform.position) * 0.05f;
		//Quaternion rotation = Quaternion.AngleAxis(180.0f, Vector3.up);
		newpos = new Vector3(-newpos.x, newpos.y, -newpos.z);
		newpos = newpos + target.transform.position;
		return newpos;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
