using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkUtils {
	
	public static string serverUrl = "http://localhost:8000/images/";
	
	public static IEnumerator PostParameters(List<string> paras){
		WWWForm form = new WWWForm();
		form.AddField("ambient", paras[0]);
		form.AddField("cdom", paras[1]);
		form.AddField("depth", paras[2]);
		form.AddField("frame_step", paras[3]);
		form.AddField("fresnel_coeff", paras[4]);
		form.AddField("influx", paras[5]);
		form.AddField("wave_scale", paras[6]);
		WWW postform = new WWW(serverUrl+"post_parameter", form);
		yield return postform;
		if(!string.IsNullOrEmpty(postform.error)){
			Debug.Log(postform.error);
		}else{
			Debug.Log(postform.text);
		}
		
	}
}
