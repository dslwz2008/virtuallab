using UnityEngine;
using System.Collections;

public class MainMenuVik : MonoBehaviour
{

    void Awake()
    {
        //PhotonNetwork.logLevel = NetworkLogLevel.Full;

        //Connect to the main photon server. This is the only IP and port we ever need to set(!)
        if (!PhotonNetwork.connected)
            PhotonNetwork.ConnectUsingSettings("v1.0"); // version of the game/demo. used to separate older clients from newer ones (e.g. if incompatible)

        //Load name from PlayerPrefs
        PhotonNetwork.playerName = PlayerPrefs.GetString("playerName", "参与者" + Random.Range(1, 9999));
		
        //Set camera clipping for nicer "main menu" background
        Camera.main.farClipPlane = Camera.main.nearClipPlane + 0.1f;
		gameManager = GetComponent<GameManagerVik>();
    }

    private string roomName = "实验室";
    private Vector2 scrollPos = Vector2.zero;
	private int selModelID = 0;
	private string[] models = {"老奶奶", "男士", "女士", "男孩", "女孩"};
	private GameManagerVik gameManager;

    void OnGUI()
    {
        if (!PhotonNetwork.connected)
        {
            ShowConnectingGUI();
            return;   //Wait for a connection
        }


        if (PhotonNetwork.room != null)
            return; //Only when we're not in a Room


        GUILayout.BeginArea(new Rect((Screen.width - 400) / 2, (Screen.height - 300) / 2, 400, 500));

        GUILayout.Label("主菜单");

        //Player name
        GUILayout.BeginHorizontal();
        GUILayout.Label("参与者名字:", GUILayout.Width(150));
        PhotonNetwork.playerName = GUILayout.TextField(PhotonNetwork.playerName);
        if (GUI.changed)//Save name
            PlayerPrefs.SetString("playerName", PhotonNetwork.playerName);
        GUILayout.EndHorizontal();

        GUILayout.Space(15);


        //Join room by title
        GUILayout.BeginHorizontal();
        GUILayout.Label("加入实验室:", GUILayout.Width(150));
        roomName = GUILayout.TextField(roomName);
        if (GUILayout.Button("开始"))
        {
            PhotonNetwork.JoinRoom(roomName);
        }
        GUILayout.EndHorizontal();

        //Create a room (fails if exist!)
        GUILayout.BeginHorizontal();
        GUILayout.Label("创建实验室:", GUILayout.Width(150));
        roomName = GUILayout.TextField(roomName);
        if (GUILayout.Button("开始"))
        {
            PhotonNetwork.CreateRoom(roomName, true, true, 10);
        }
        GUILayout.EndHorizontal();

        //Join random room
        GUILayout.BeginHorizontal();
        GUILayout.Label("随机加入房间:", GUILayout.Width(150));
        if (PhotonNetwork.GetRoomList().Length == 0)
        {
            GUILayout.Label("现在还没有房间");
        }
        else
        {
            if (GUILayout.Button("开始"))
            {
                PhotonNetwork.JoinRandomRoom();
            }
        }
        GUILayout.EndHorizontal();
		
		GUILayout.Space(15);
		GUILayout.BeginHorizontal();
		selModelID = GUILayout.SelectionGrid(selModelID, models, 2);
		if(GUI.changed){
			switch(selModelID){
			case 0:
				gameManager.playerPrefabName = "grannyprefab";
				break;
			case 1:
				gameManager.playerPrefabName = "manprefab";
				break;
			case 2:
				gameManager.playerPrefabName = "womanprefab";
				break;
			case 3:
				gameManager.playerPrefabName = "boyprefab";
				break;
			case 4:
				gameManager.playerPrefabName = "girlprefab";
				break;
			default:
				break;
			}
		}
		GUILayout.EndHorizontal();

        GUILayout.Space(30);
        GUILayout.Label("房间列表:");
        if (PhotonNetwork.GetRoomList().Length == 0)
        {
            GUILayout.Label("现在还没有房间");
        }
        else
        {
            //Room listing: simply call GetRoomList: no need to fetch/poll whatever!
            scrollPos = GUILayout.BeginScrollView(scrollPos);
            foreach (RoomInfo game in PhotonNetwork.GetRoomList())
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(game.name + " " + game.playerCount + "/" + game.maxPlayers);
                if (GUILayout.Button("加入"))
                {
                    PhotonNetwork.JoinRoom(game.name);
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
        }

        GUILayout.EndArea();
    }


    void ShowConnectingGUI()
    {
        GUILayout.BeginArea(new Rect((Screen.width - 400) / 2, (Screen.height - 300) / 2, 400, 300));

        GUILayout.Label("正在连接到Photon服务器.");
        //GUILayout.Label("Hint: This demo uses a settings file and logs the server address to the console.");

        GUILayout.EndArea();
    }
}
