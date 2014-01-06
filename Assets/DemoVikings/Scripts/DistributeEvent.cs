using UnityEngine;
using System.Collections;


public class DistributeEvent : MonoBehaviour{
		
	public delegate void MouseDown_Delegate();
	public delegate void TextureAnimation_Delegate();
	
	public MouseDown_Delegate changeTexture;
	public MouseDown_Delegate playVideo;
	
	public TextureAnimation_Delegate animateTexture;
	
}
