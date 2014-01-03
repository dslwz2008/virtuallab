using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

	void Start() {
        StartCoroutine("DoSomething", 2.0F);
        //StopCoroutine("DoSomething");
    }
    IEnumerator DoSomething(float someParameter) {
        while (true) {
            print("DoSomething Loop" + Time.time);
        	yield return new WaitForSeconds(someParameter);
		}
    }
	
//	 void Start() {
//        print("Starting " + Time.time);
//        StartCoroutine(WaitAndPrint(2.0F));
//        print("Before WaitAndPrint Finishes " + Time.time);
//    }
//    IEnumerator WaitAndPrint(float waitTime) {
//        yield return new WaitForSeconds(waitTime);
//        print("WaitAndPrint " + Time.time);
//    }
}
