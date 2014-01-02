using UnityEngine;
using System.Collections;

public class DistributeEvent{
	
	public delegate void MouseDown_Delegate();
	public static MouseDown_Delegate changeTexture;
	public static MouseDown_Delegate playVideo;
	
}
