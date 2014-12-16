using UnityEngine;
using System.Collections;

public class ReadAndParseDataFromDB : MonoBehaviour {

	private SqliteDB sqlitedb = null;
	//private string dbName = "agents";
	public string dbName = "agent_escalator";
	public string tableName = "agentlocation";
	
	public float readInterval = 10.0f;
	public int linesPerRead = 10000;
	private int startLine = 0;
	
	public GameObject source = null;
	public GameObject target = null;
	
	// Use this for initialization
	void OnEnable () {
		sqlitedb = new SqliteDB();
		sqlitedb.OpenDB(dbName);		
		InvokeRepeating("ReadOnce", 0f, readInterval);
	}
	
	void OnDisable(){
		CancelInvoke("ReadOnce");
		sqlitedb.CloseDB();
		startLine = 0;
	}
	
	public void ReInitialiseReader(){
		CancelInvoke("ReadOnce");
		InvokeRepeating("ReadOnce", 0f, readInterval);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void ReadOnce(){
		ArrayList result = sqlitedb.ReadLimitLines(tableName, startLine, linesPerRead);
		//there is no data
		if(result.Count == 0){
			Debug.Log("data finish!");
			sqlitedb.CloseDB();
			CancelInvoke("ReadOnce");
			return;
		}
		
		int endLine = 0;
		if(result.Count < linesPerRead){
			endLine = startLine + result.Count;
		}else{
			endLine = startLine + linesPerRead;
		}
		
		int i = 0;
		int frameid = 1;//from 1
		while(true){
			ArrayList temp = result[i] as ArrayList;
			int recordID = int.Parse(temp[0].ToString());
			int linesInThisFrame = int.Parse(temp[1].ToString());//frame numbers
			//lack of data, prepare next read from db
			if((endLine - recordID) < linesInThisFrame){
				startLine = recordID - 1;//very important
				break;
			}
			
			Frame frame = new Frame();
			frame.id = frameid;
			for(int j = 1; j <= linesInThisFrame; j++){
				ArrayList data = result[i+j] as ArrayList;
				int peopleID = int.Parse(data[1].ToString());
				int peopleStatus = int.Parse(data[2].ToString());
				float posx = float.Parse(data[3].ToString());
				float posy = 8.4f;//最上层//float.Parse(data[4].ToString());
				float posz = float.Parse(data[4].ToString());
				People p = new People(peopleID, peopleStatus, PreTransform(new Vector3(posx,posy,posz)));
				frame.peoples.Add(peopleID, p);
			}
			FrameBuffer.GetInstance().GetQueue().Enqueue(frame);
			frameid++;
			i += (linesInThisFrame+1);
			
			//data over
			if(i >= result.Count){
				startLine = endLine;//very important
				break;
			}
		}
		
	}
	
	Vector3 PreTransform(Vector3 pos){
		Vector3 newpos = (pos - source.transform.position) * 0.05f;
		//Quaternion rotation = Quaternion.AngleAxis(180.0f, Vector3.up);
		newpos = new Vector3(-newpos.x, newpos.y, -newpos.z);
		newpos = newpos + target.transform.position;
		return newpos;
	}
}
