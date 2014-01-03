using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SimulateFlood : MonoBehaviour {
	
	private bool needSetParameter = false;
	private bool parameterError = false;
	private string ambient = "", cdom = "", depth = "", frame_step = "", 
		fresnel_coeff = "", influx = "", wave_scale = "";
	
	// Use this for initialization
	void Start () {
	
	}
	
	void OnMouseDown(){
        Camera.main.farClipPlane = Camera.main.nearClipPlane + 0.1f;
		needSetParameter = true;
	}
	
	void OnGUI(){
		if(needSetParameter){
			GUILayout.BeginArea(new Rect((Screen.width - 400) / 2, (Screen.height - 300) / 2, 400, 500));
			GUILayout.Label("Please input your flood parameter: ");
			
			GUILayout.BeginHorizontal();
			GUILayout.Label("ambient", GUILayout.Width(100));
			ambient = GUILayout.TextField(ambient, 5, GUILayout.Width(100));
			GUILayout.Label("interval [0.1,1.0]");
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			GUILayout.Label("cdom", GUILayout.Width(100));
			cdom = GUILayout.TextField(cdom, 5, GUILayout.Width(100));
			GUILayout.Label("interval [0.1,1.0]");
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			GUILayout.Label("depth", GUILayout.Width(100));
			depth = GUILayout.TextField(depth, 5, GUILayout.Width(100));
			GUILayout.Label("interval [1.0,100.0]");
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			GUILayout.Label("frame_step", GUILayout.Width(100));
			frame_step = GUILayout.TextField(frame_step, 5, GUILayout.Width(100));
			GUILayout.Label("interval [1.0,50.0]");
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			GUILayout.Label("fresnel_coeff", GUILayout.Width(100));
			fresnel_coeff = GUILayout.TextField(fresnel_coeff, 5, GUILayout.Width(100));
			GUILayout.Label("interval [0.0,1.0]");
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			GUILayout.Label("influx", GUILayout.Width(100));
			influx = GUILayout.TextField(influx, 5, GUILayout.Width(100));
			GUILayout.Label("interval [10,300]");
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			GUILayout.Label("wave_scale", GUILayout.Width(100));
			wave_scale = GUILayout.TextField(wave_scale, 5, GUILayout.Width(100));
			GUILayout.Label("interval [0.0,1.0]");
			GUILayout.EndHorizontal();
			
			if(parameterError){
				GUILayout.Label("Please check your parameter!");
			}
			
			GUILayout.BeginHorizontal();
			if(GUILayout.Button("OK")){
				//valid check 
				if(isValidParameter()){
					Debug.Log("no problem with parameter.");
					List<string> paras = new List<string>();
					paras.Add(ambient);
					paras.Add(cdom);
					paras.Add(depth);
					paras.Add(frame_step);
					paras.Add(fresnel_coeff);
					paras.Add(influx);
					paras.Add(wave_scale);
					StartCoroutine(NetworkUtils.PostParameters(paras));
					
					Camera.main.farClipPlane = 1000;
					needSetParameter = false;
					parameterError = false;
					
					GameObject.Find("/Scripts").GetComponent<ListenNetwork>().enabled = true;
				}else{
					parameterError = true;
				}
			}
			if(GUILayout.Button("Cancel")){
				Camera.main.farClipPlane = 1000;
				needSetParameter = false;
				parameterError = false;
			}
			GUILayout.EndHorizontal();
			
			GUILayout.EndArea();
		}

	}
	
	bool isValidParameter(){
		try{
			float fAmbient = float.Parse(ambient);
			float fCdom = float.Parse(cdom);
			float fDepth = float.Parse(depth);
			float fFrame_step = float.Parse(frame_step);
			float fFresnel_coeff = float.Parse(fresnel_coeff);
			float fInflux = float.Parse(influx);
			float fWave_scale = float.Parse(wave_scale);
			if(fAmbient >= 0.1f && fAmbient <= 1.0f && 
				fCdom >= 0.1f && fCdom <= 1.0f &&
				fDepth >= 1.0f && fDepth <= 100.0f &&
				fFrame_step >= 1.0f && fFrame_step <= 50.0f &&
				fFresnel_coeff >= 0.0f && fFresnel_coeff <= 1.0f &&
				fInflux >= 10f && fInflux <= 300 &&
				fWave_scale >= 0.0f && fWave_scale <= 1.0f){
				return true;
			}else{
				return false;
			}
		}catch(Exception ex){
			return false;
		}
	}
}
