using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Server : NetworkManager {

	
	private GameObject ball;
	public int clientCount = 0;
	public int userCount;
	public bool userCheck = true;
	public Button button1, button2;
	void Start()
	{
		ball = spawnPrefabs[0];
		// maxConnections = 1;
		
	}
	public override void OnStartServer(){

		base.OnStartServer();
		Debug.Log("[OnStartServer] Start Server success \n ip = " + networkAddress + " : " + networkPort);

		if (NetworkServer.active) {
			// NetworkServer.active = true;
			// sc.CmdsetCheck();
			Debug.Log("server active");
			// CmdsetActive();
		}else {

			Debug.Log("server not active");
		}
	
		
		onlineScene = "main";
		
		
	}


	public void showBall() {
			
		// if (!NetworkServer.active) {
		// 	return;
		// }
		Vector3 pos = new Vector3(0f,1.5f,0f);

		GameObject b = Instantiate(ball, pos , Quaternion.identity);
		// e.transform.LookAt(Vector3.zero);
		// e.GetComponent<Rigidbody>().AddForce(e.transform.forward * 10f);
		int r = Random.Range(0,2);
		if (r==0){
			b.transform.eulerAngles = new Vector3(0f,180f,0); 
		}
			
		b.GetComponent<Rigidbody>().velocity = b.transform.forward * 6f;
		// e.GetComponent<Rigidbody>().AddForce(e.transform.forward * 5f);
		NetworkServer.Spawn(b);
	}

	IEnumerator showB() {
		yield return new WaitForSeconds(2.0f);
		showBall();
	}


	//클라이언트가 들어왔을때 서버에서 호출
	public override void OnServerConnect(NetworkConnection conn) {
		
		clientCount++;

		if (clientCount == 2) {

			StartCoroutine (showB() );
		}
		base.OnServerConnect(conn);
		Debug.Log("[OnServerConnect] Client Connect ip = " + conn.address );
		// NetworkServer.dontListen = true;
		var x = NetworkClient.allClients;
		Debug.Log("client lenght = " + x.Count );
	}

	//클라이언트가 나갔을때 서버에서 호출
	public override void OnServerDisconnect(NetworkConnection conn) {
		
		// userCheck = false;
		clientCount--;
		base.OnServerDisconnect(conn);
		Debug.Log("[OnServerDisconnect] Client Disconnect ip = " + conn.address );

	}

	public override void OnServerError(NetworkConnection conn, int errorCode) {

		// base.OnServerError(conn,errorCode);
		Debug.Log("[OnServerError] Server Error , Error code = " + errorCode );
		// offlineScene = "main";
	}

	public override void OnClientConnect(NetworkConnection conn) {

		base.OnClientConnect(conn);
		Debug.Log("[OnClientConnect] server ip = " + conn.address);
		Debug.Log("server check22 = " + NetworkServer.active);
		// onlineScene = "main";
		button1 = GameObject.Find("Exit1").GetComponent<Button>();
		button2 = GameObject.Find("Exit2").GetComponent<Button>();
		button1.onClick.AddListener(ExitClient);
		button2.onClick.AddListener(ExitClient);
	}


	public override void OnStartClient(NetworkClient client) {

		base.OnStartClient(client);
		Debug.Log("start client");
		client.RegisterHandler(MsgType.Disconnect, ConnectionError);
		// onlineScene = "main";
		
		
	}

	// public override void OnStopClient() {

	// 	offlineScene = "setting";
	// }



	public override void OnClientError(NetworkConnection conn, int errorCode) {

		base.OnClientError(conn, errorCode);
		Debug.Log("[OnClientError] error code = " + errorCode );
	}

	public override void OnClientDisconnect(NetworkConnection conn) {

		base.OnClientDisconnect(conn);
		Debug.Log("OnClientDisconnect ip = " + conn.address );
		
	}

	public override void OnClientNotReady(NetworkConnection conn) {

		Debug.Log("[OnClientNotReady] ddd");
	}


	public override void OnServerReady(NetworkConnection conn) {
		Debug.Log("[OnServerReady] ddd");
	}

	
	public void ExitClient() {

		StopClient();
		Application.Quit();
	}
	public virtual void ConnectionError() {
		Debug.Log("connection error");
	}
	public void ConnectionError(NetworkMessage netMsg) {
		Debug.Log("connection error");
	}

	public void resetScene() {

		SceneManager.LoadScene(0);
	}

}
