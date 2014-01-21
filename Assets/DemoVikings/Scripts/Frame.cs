using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// store frame data. need new object
/// </summary>
public class Frame{
	public Frame(){
		this.id = 0;
		this.peoples = new Dictionary<int, People>();
	}
	
	/// <summary>
	/// frame id
	/// </summary>
	public int id;
	
	/// <summary>
	/// The people.
	/// </summary>
	public Dictionary<int, People> peoples = null; 
	
}
