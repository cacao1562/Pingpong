using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Hand : NetworkBehaviour {

	///터치할때 카메라
	public Camera cam; 
	private Rigidbody rb;
	 ///1번이 레드 , 2번 블루
	public Material red, blue;
	///플레이어 번호
	private int number; 
	private GameObject position1, position2;
	public BoxCollider boxCol;
	///1번 스코어
	[SyncVar (hook = "addPlayer1") ] 
	public int score1;
	///2번 스코어
	[SyncVar (hook = "addPlayer2") ] 
	public int score2;
	[Command ]
	public void CmdAddScore1() {
		score1++;
	}
	[Command]
	public void CmdAddScore2() {
		score2++;
	}

	private Text textP11, textP12, textP21, textP22;

	void addPlayer1(int n) { //스코어1 콜백

		textP11.text = n.ToString();
		textP21.text = n.ToString();
	}

	void addPlayer2(int n) { //스코어2 콜백

		textP12.text = n.ToString();
		textP22.text = n.ToString();
	}
	void Start () {

		Screen.sleepTimeout = SleepTimeout.NeverSleep;

		rb = GetComponent<Rigidbody>();
		Server ss = GameObject.Find("NetworkManager").GetComponent<Server>(); //ss.spawnPrefabs[0] 가져올수있음
		position1 = GameObject.Find("startPosition"); //1번 플레이어 위치
		position2 = GameObject.Find("startPosition2"); //2번 플레이어 위치
		boxCol = GetComponent<BoxCollider>();
		GameObject camera1 = GameObject.Find("Main Camera"); //1번 플레이어 카메라
		GameObject camera2 = GameObject.Find("Main Camera2"); //2번 플레이어 카메라
		textP11 = camera1.transform.GetChild(0).Find("score1").GetComponent<Text>();
		textP12 = camera1.transform.GetChild(0).Find("score2").GetComponent<Text>();
		textP21 = camera2.transform.GetChild(0).Find("score1").GetComponent<Text>();
		textP22 = camera2.transform.GetChild(0).Find("score2").GetComponent<Text>();

		ss.userCount++;	
		number = ss.userCount;

		if (number == 1) {

			GetComponent<MeshRenderer>().material = red;
			gameObject.tag = "player1";

		}else if (number == 2) {

			GetComponent<MeshRenderer>().material = blue;
			gameObject.tag = "player2";
		}else if (number == 3) {

			GetComponent<MeshRenderer>().material = red;
			gameObject.tag = "player3";
		}else if (number == 4) {

			GetComponent<MeshRenderer>().material = blue;
			gameObject.tag = "player4";
		}

		if (isLocalPlayer) {

			// ss.findCamera();
			if (number == 1 || number == 3) {
				
				transform.position = new Vector3(position1.transform.position.x, 1.1f, position1.transform.position.z);
				cam = position1.transform.GetChild(0).GetComponent<Camera>();  //터치 할때 카메라
				camera1.GetComponent<Camera>().depth = 0; // 화면 뷰 카메라
				
			} else if (number == 2 || number == 4){
					
				transform.position = new Vector3(position2.transform.position.x, 1.1f, position2.transform.position.z);
				cam = position2.transform.GetChild(0).GetComponent<Camera>();
				camera2.GetComponent<Camera>().depth = 0;
			}
		}
		
		
	}


	public float minCuttingVelocity = 0.001f;
	
	// private Vector2 previousPosition;

	private Vector2 vv;
	private Vector2 move;
	///터치다운 중일때
	private bool isDrag;


	void Update () {

		if (!isLocalPlayer) {
			return;
		}

		if (Input.GetMouseButtonDown(0) ) {
			
			vv = cam.ScreenToWorldPoint(Input.mousePosition);
			isDrag = true;

		} else if (Input.GetMouseButtonUp(0) ) {

			isDrag = false;
			
		}

		if (isDrag) {

			vv = move;
			move = cam.ScreenToWorldPoint(Input.mousePosition);

		}
	
		
	}

	
	void FixedUpdate()
	{
		if (isDrag) {

			if (number == 1 || number == 3) {

				transform.localEulerAngles = new Vector3(0f, 0f, -(move.x * 10f)); //라켓 기울어지게 왼쪽 오른쪽으로
				float z = -5.8f + move.y;
				float zz = Mathf.Clamp(z, -5.5f, -1.0f); //움직이는 범위
				Vector3 tt = Vector3.MoveTowards(transform.position, new Vector3(move.x, 0.5f, zz), 0.5f );
				rb.MovePosition( tt ); 
				// rb.MovePosition(new Vector3(move.x, 0.5f, zz ));  // 포지션으로 움직이면 순간이동되서 
				direction = new Vector3(move.x - vv.x, move.y - vv.y, 0f).normalized;
				// float m = (move - vv).magnitude;
				// direction.z = 0.9f;
				// float mm = (move - vv).magnitude * Time.deltaTime;
				// Debug.Log("m = " + m );
				// Debug.Log("mm = " + mm );
				
			}else if (number == 2 || number == 4) {

				transform.localEulerAngles = new Vector3(0f, 0f, -(move.x * 10f)); 
				float z = 5.8f - move.y;
				float zz = Mathf.Clamp(z, 1.0f, 5.5f);
				Vector3 tt = Vector3.MoveTowards(transform.position, new Vector3(move.x, 0.5f, zz), 0.5f );
				rb.MovePosition( tt ); 
				// rb.MovePosition(new Vector3(move.x, 1.0f, zz ));
				direction = new Vector3(move.x - vv.x, move.y - vv.y, 5f).normalized;
				
			}

			
		}else {

			rb.velocity = Vector3.zero;
		}
	}


	///드래그한 방향	
	private Vector3 direction;
	private float x;

	void OnCollisionEnter(Collision other)
	{

		// rb.velocity = Vector3.zero;	
		// if (!isLocalPlayer) { return; }

		if (other.collider.tag == "ball") {

			boxCol.enabled = false;
			other.transform.position = new Vector3(other.transform.position.x, 1f, other.transform.position.z );
			other.transform.rotation = transform.rotation;
			// other.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
			// transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
			
			if (number == 1 || number == 3) {
				
				// if (direction.x > 0f) {
				// 	direction.x += 0.1f;
				// 	x = direction.x;
				// }else if (direction.x < 0f ){
				// 	direction.x -= 0.1f;
				// 	x = direction.x;
				// }

				x = direction.x * 2;
				Vector3 v3 = GetVelocity(new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.2f), new Vector3(x, 1f, 1.5f), 20f);
				other.transform.GetComponent<Rigidbody>().velocity = v3 ;
				// direction.y = 0.2f; //Mathf.Sin(10 * Mathf.PI / 180 );
				// direction.z = 1.0f;
				// boxCol.enabled = true;
				StartCoroutine( enableCol() );


			}else if (number == 2 || number == 4) {
				
				// if (direction.x > 0f) {
				// 	direction.x += 0.2f;
				// 	x = direction.x;
				// }else if (direction.x < 0f ){
				// 	direction.x -= 0.2f;
				// 	x = direction.x;
				// }
				x = direction.x * 2;
				Vector3 v3 = GetVelocity(new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.2f), new Vector3(x, 1f, -1.5f), 20f);
				other.transform.GetComponent<Rigidbody>().velocity = v3;

				StartCoroutine( enableCol() );
				
			}
			

			
		}
	
		// Vector2 dir = other.transform.GetComponent<Rigidbody>().velocity;
		// other.transform.GetComponent<Rigidbody>().velocity = v2Rotate( -dir, 2 * getAngle(dir, (transform.position - other.transform.position)));
	}

	private IEnumerator enableCol() {
		yield return new WaitForSeconds(0.4f);
		boxCol.enabled = true;
	}




	Vector3 GetVelocity(Vector3 currentPos, Vector3 targetPos, float initialAngle)
	{
		float gravity = Physics.gravity.magnitude;
		float angle = initialAngle * Mathf.Deg2Rad;
	
		Vector3 planarTarget = new Vector3(targetPos.x, 0, targetPos.z);
		Vector3 planarPosition = new Vector3(currentPos.x, 0, currentPos.z);
	
		float distance = Vector3.Distance(planarTarget, planarPosition);
		float yOffset = currentPos.y - targetPos.y;
	
		float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));
	
		Vector3 velocity = new Vector3(0f, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));
	
		float angleBetweenObjects = Vector3.Angle(Vector3.forward, planarTarget - planarPosition) * (targetPos.x > currentPos.x ? 1 : -1);
		Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;
	
		return finalVelocity;
	}


	// float getAngle(Vector2 vec1, Vector2 vec2) {

	// 	float angle = (Mathf.Atan2(vec2.y, vec2.x) - Mathf.Atan2(vec1.y, vec1.x)) * Mathf.Rad2Deg;
	// 	return angle;
	// }

	// Vector2 v2Rotate(Vector2 aPoint, float aDegree) {

	// 	float rad = aDegree * Mathf.Deg2Rad;
	// 	float s = Mathf.Sin(rad);
	// 	float c = Mathf.Cos(rad);
	// 	return new Vector2( aPoint.x * c - aPoint.y * s, aPoint.y * c + aPoint.x * s);
	// }



}
