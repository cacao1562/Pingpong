using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Ball : NetworkBehaviour {

	// Use this for initialization

	private Server server;
	public GameObject p1, p2, p3, p4;
	private Hand hand1, hand2, hand3, hand4;
	void Start () {
		
		if (!isServer) {
			 return;
		}

		server = GameObject.Find("NetworkManager").GetComponent<Server>();

		if (!server.userCheck) {
			return;
		}


		p1 = GameObject.FindWithTag("player1");
		p2 = GameObject.FindWithTag("player2");
		p3 = GameObject.FindWithTag("player3");
		p4 = GameObject.FindWithTag("player4");

		if (p1 != null) {
			hand1 = p1.GetComponent<Hand>();
		}
		if (p2 != null) {
			hand2 = p2.GetComponent<Hand>();
		}
		if (p3 != null) {
			hand3 = p3.GetComponent<Hand>();
		}
		if (p4 != null) {
			hand4 = p4.GetComponent<Hand>();
		}
		

		

	}
	
	private bool check = true;
	public int score1 , score2;
	void Update () {

		if (!isServer) {
			 return;
		}

		if (!server.userCheck) {
			return;
		}

		if (check) {

			if (transform.position.y < -1) {

				if (transform.position.z < 0) {
					//p2 win
					score2++;
					// server.winPlayer2();
					hand2.CmdAddScore2();
					Destroy(gameObject);
					server.showBall();		
					check = false;
				}
				if (transform.position.z > 0 ) {
					//p1 win
					score1++;
					// server.winPlayer1();
					hand1.CmdAddScore1();
					Destroy(gameObject);
					server.showBall();
					check = false;
				}
			}
		}
		
		
	}

	
	public int player;
	public int pingCount;
	

	void OnCollisionEnter(Collision other)
	{
		if (!isServer) {
			 return;
		}

		if (!server.userCheck) {
			return;
		}
		// if (isLocalPlayer) {
			if (other.collider.tag == "player1" || other.collider.tag == "player3") {
				
				player = 1;
				pingCount = 0;
				
			}else if (other.collider.tag == "player2" || other.collider.tag == "player4") {

				player = 2;
				pingCount = 0;
				
			}else if (other.collider.tag == "table1") {
	//			
				
				pingCount++;
					
				if (pingCount >= 2) {
					// pl lose
					score2++;
					// server.winPlayer2();
					hand2.CmdAddScore2();
					Destroy(gameObject);
					server.showBall();
				}
				

			}else if (other.collider.tag == "table2") {

				pingCount++;
					
				if (pingCount >= 2) {
					// p2 lose
					score1++;
					// server.winPlayer1();
					hand1.CmdAddScore1();
					Destroy(gameObject);
					server.showBall();
				}
				
			}
		// }
	
	}



}
