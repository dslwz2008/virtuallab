using UnityEngine;
using System.Collections;

public class TransformTest : MonoBehaviour {
	public GameObject source = null;
	public GameObject target = null;

	// Use this for initialization
	void Start () {
		source.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
		source.transform.Rotate(0.0f, 180.0f, 0.0f);
		Vector3 offset = new Vector3(source.transform.position.x - target.transform.position.x, 
			target.transform.position.y,
			source.transform.position.z - target.transform.position.z);
		source.transform.Translate(offset);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
