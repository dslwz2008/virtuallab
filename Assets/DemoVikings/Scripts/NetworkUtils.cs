using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkUtils {
	
	public static string serverUrl = "http://localhost:8000/images/";
	public static int requestIntervel = 0.5;
	private static string paraName = "";
	private static int startNumber = 0, endNumber = 0;
	public static ArrayList<Texture2D> textures = new ArrayList<Texture2D>();
	
	public static IEnumerator PostParameters(List<string> paras){
		paraName = NetworkUtils.ParameterToString(paras);
		WWWForm form = new WWWForm();
		form.AddField("ambient", paras[0]);
		form.AddField("cdom", paras[1]);
		form.AddField("depth", paras[2]);
		form.AddField("frame_step", paras[3]);
		form.AddField("fresnel_coeff", paras[4]);
		form.AddField("influx", paras[5]);
		form.AddField("wave_scale", paras[6]);
		WWW postform = new WWW(serverUrl + "post_parameter", form);
		yield return postform;
		if(!string.IsNullOrEmpty(postform.error)){
			Debug.Log(postform.error);
		}else{
			Debug.Log(postform.text);
		}
	}
	
	public static IEnumerator ListenAndHandle(){
		bool needStop = true;
		while(needStop){
			WWWForm form = new WWWForm();
			form.AddField("paraname", paraName);
			WWW postform = new WWW(serverUrl + "get_status", form);
			yield return postform;
			if(!string.IsNullOrEmpty(postform.error)){
				Debug.Log(postform.error);
			}else{
				Debug.Log(postform.text);
				//examples:
				//status:0;number:0
				//status:1;number:15
				//status:2;number:200
				string[] pairs = postform.text.Split(";");
				if(pairs.Length == 1){
					Debug.Log("出错了！");
					return null;
				}
				string status = pairs[0].Split(":")[1];
				endNumber = int.Parse(pairs[1].Split(":")[1]);
				switch(status){
				case "0"://还没有回应，继续请求
					yield return WaitForSeconds(requestIntervel);
					break;
				case "1"://没有回应完成，先取部分图片，继续请求
					for(int index = startNumber; index < endNumber; index++){
						string getImageUrl = serverUrl + "get_image?paraname=" + paraName + "&imageorder=" + index.ToString();
						WWW getimage = new WWW(getImageUrl);
						yield return getimage;
						textures.Append(getimage.texture);
					}
					startNumber = endNumber;
					break;
				case "2"://回应结束了，把剩下的图片全部取出，停止请求
					if(textures.Count < endNumber){
						for(int index = textures.Count; index < endNumber; index++){
							string getImageUrl = serverUrl + "get_image?paraname=" + paraName + "&imageorder=" + index.ToString();
							WWW getimage = new WWW(getImageUrl);
							yield return getimage;
							textures.Append(getimage.texture);
						}
					}
					startNumber = endNumber = 0;
					needStop = true;
					break;
				default://有错误
					needStop = true;
					break;
				}
			}
		}
	}
	
	public static string ParameterToString(List<string> paras){
		string result = paras[0];
		for(int i = 1; i < paras.Count; i++){
			result += ("_"+paras[i]);
		}
		return result;
	}
}
