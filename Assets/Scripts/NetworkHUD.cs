using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NetworkHUD : MonoBehaviour {

	public InputField inputField;
	private NetworkManager manager;
	void Awake()
	{
		manager = GetComponent<Server>();
	}
	void Start () {
		inputField.text = "192.168.0.23";
	}
	
	public void startHost() {
		
		manager.StartHost();
	}

	public void startClient() {
		
		if (inputField.text == "") {
			return;
		}
		manager.networkAddress = inputField.text;
		 
		manager.StartClient();

		Debug.Log( "aa = " + Application.loadedLevelName) ;
		Debug.Log( "ss = " + SceneManager.GetActiveScene().name );
	}
	public void startServer() {
		manager.StartServer();
	}
}
