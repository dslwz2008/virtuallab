using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SimulateFlood : MonoBehaviour {
	
	private bool needSetParameter = false;
	private bool parameterError = false;
	private string ambient = "0.5", cdom = "0.5", depth = "50", frame_step = "25", 
		fresnel_coeff = "0.5", influx = "200", wave_scale = "0.5";
	private string serverUrl = "193.168.1.100";
	
	private NetworkUtils nutils = null;
	
	private Shader diffuse = null;
	private Shader selfillumin = null;
	
	// Use this for initialization
	void Start () {
		nutils = GameObject.Find("/Scripts").GetComponent<NetworkUtils>();
		
		diffuse = Shader.Find("Diffuse");
		selfillumin = Shader.Find("Self-Illumin/Diffuse");
	}
	
	void OnMouseEnter(){
		renderer.material.shader = selfillumin;
	}
	
	void OnMouseExit(){
		renderer.material.shader = diffuse;
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
			GUILayout.Label("环境光", GUILayout.Width(100));
			ambient = GUILayout.TextField(ambient, 5, GUILayout.Width(100));
			GUILayout.Label("区间 [0.1,1.0]");
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			GUILayout.Label("有机污染物含量", GUILayout.Width(100));
			cdom = GUILayout.TextField(cdom, 5, GUILayout.Width(100));
			GUILayout.Label("区间 [0.1,1.0]");
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			GUILayout.Label("深度", GUILayout.Width(100));
			depth = GUILayout.TextField(depth, 5, GUILayout.Width(100));
			GUILayout.Label("区间 [1.0,100.0]");
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			GUILayout.Label("渲染速度", GUILayout.Width(100));
			frame_step = GUILayout.TextField(frame_step, 5, GUILayout.Width(100));
			GUILayout.Label("区间 [1.0,50.0]");
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			GUILayout.Label("菲斯涅尔系数", GUILayout.Width(100));
			fresnel_coeff = GUILayout.TextField(fresnel_coeff, 5, GUILayout.Width(100));
			GUILayout.Label("区间 [0.0,1.0]");
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			GUILayout.Label("流量", GUILayout.Width(100));
			influx = GUILayout.TextField(influx, 5, GUILayout.Width(100));
			GUILayout.Label("区间 [10,300]");
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			GUILayout.Label("波浪", GUILayout.Width(100));
			wave_scale = GUILayout.TextField(wave_scale, 5, GUILayout.Width(100));
			GUILayout.Label("区间 [0.0,1.0]");
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			GUILayout.Label("服务器地址http://", GUILayout.Width(100));
			serverUrl = GUILayout.TextField(serverUrl, 50, GUILayout.Width(100));
			GUILayout.Label("例如:192.168.1.100");
			GUILayout.EndHorizontal();
			
			if(parameterError){
				GUILayout.Label("Please check your parameter!");
			}
			
			GUILayout.BeginHorizontal();
			if(GUILayout.Button("OK")){
				//valid check 
				if(isValidParameter()){
					nutils.serverUrl = "http://" + serverUrl + ":8000/images/";
					Debug.Log("no problem with parameter.");
					List<string> paras = new List<string>();
					paras.Add(ambient);
					paras.Add(cdom);
					paras.Add(depth);
					paras.Add(frame_step);
					paras.Add(fresnel_coeff);
					paras.Add(influx);
					paras.Add(wave_scale);
					StartCoroutine(nutils.PostParameters(paras));
					
					Camera.main.farClipPlane = 1000;
					needSetParameter = false;
					parameterError = false;
					
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
				fWave_scale >= 0.0f && fWave_scale <= 1.0f &&
				Utilities.IsValidIP(serverUrl)){
				return true;
			}else{
				return false;
			}
		}catch(Exception ex){
			return false;
		}
	}
}

