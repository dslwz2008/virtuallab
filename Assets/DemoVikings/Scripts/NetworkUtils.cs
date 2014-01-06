using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkUtils : Photon.MonoBehaviour{
	//texture buffer
	public static List<Texture2D> textures = new List<Texture2D>();
	
	public  string serverUrl = "";
	public float requestIntervel = 2.0f;
	private string paraName = "";
	private int startNumber = 0, endNumber = 0;
	private bool needStop = false;
	private DistributeEvent de = null;
	
	void Start(){
		de = GameObject.Find("/Scripts").GetComponent<DistributeEvent>();
	}
	
	public IEnumerator PostParameters(List<string> paras){
		paraName = ParameterToString(paras);
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
		}else{//请求成功
			Debug.Log(postform.text);
			StartCoroutine(ListenAndHandle());
		}
	}
	
	public IEnumerator ListenAndHandle(){
		while(!needStop){
			WWW getform = new WWW(serverUrl + "get_image_status?paraname=" + paraName);
			yield return getform;
			if(!string.IsNullOrEmpty(getform.error)){
				Debug.Log(getform.error);
			}else{
				Debug.Log(getform.text);
				//examples:
				//status:0;number:0
				//status:1;number:15
				//status:2;number:200
				string[] pairs = getform.text.Split(new char[]{';'});
				if(pairs.Length == 1){
					Debug.Log("出错了！");
					return false;
				}
				string status = pairs[0].Split(new char[]{':'})[1];
				endNumber = int.Parse(pairs[1].Split(new char[]{':'})[1]);
				switch(status){
				case "0"://还没有回应，继续请求
					yield return new WaitForSeconds(requestIntervel);
					break;
				case "1"://没有回应完成，先取部分图片，继续请求.for all clients
					photonView.RPC("DownloadSpecificImages", PhotonTargets.AllBuffered, startNumber, endNumber);
					startNumber = endNumber;
					de.animateTexture();
					break;
				case "2"://回应结束了，把剩下的图片全部取出，停止请求.for all clients
					photonView.RPC("DownloadSpecificImages", PhotonTargets.AllBuffered, NetworkUtils.textures.Count, endNumber);
					//startNumber = endNumber = 0;
					de.animateTexture();
					needStop = true;
					break;
				default://有错误
					needStop = true;
					break;
				}
			}
		}
	}
	
	[RPC]
	IEnumerator DownloadSpecificImages(int start, int end, PhotonMessageInfo info){
		if(start < end){
			for(int index = start; index < end; index++){
				string getImageUrl = serverUrl + "get_image?paraname=" + paraName + "&imageorder=" + index.ToString();
				WWW getimage = new WWW(getImageUrl);
				yield return getimage;
				NetworkUtils.textures.Add(getimage.texture);
			}
		}
	}
	
	public string ParameterToString(List<string> paras){
		string result = paras[0];
		for(int i = 1; i < paras.Count; i++){
			result += ("_"+paras[i]);
		}
		return result;
	}
}
