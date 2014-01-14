using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class People{
	public int m_id;
	public int m_status;
	public Vector3 m_position;
	
	public People(int id, int status, Vector3 targetPosition){
		m_id = id;
		m_status = status;
		m_position = targetPosition;
	}
}
