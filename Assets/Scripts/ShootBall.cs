using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBall : MonoBehaviour {

	public GameObject ball;
	void Start () {
		
		

		if (gameObject.name == "Cube") {

			// transform.eulerAngles = new Vector3(0f, 180f, 0f);
			StartCoroutine (produceBall());

		}else if (gameObject.name == "Cube (1)") {

			// Quaternion qq = Quaternion.identity;
			// qq.eulerAngles = new Vector3(0f, 180f, 0f);
			// transform.rotation *= qq;
			StartCoroutine (produceBall2());
		}

	}
	
	IEnumerator produceBall() {

		int i=0;
		while (i<100) {
			i++;
			GameObject b = Instantiate(ball,transform.position,transform.rotation);
			// b.GetComponent<Rigidbody>().AddForce(transform.forward * 2f, ForceMode.Impulse);
			Vector3 vv = GetVelocity(transform.position, new Vector3(transform.position.x, transform.position.y, 3f), 20f);
			b.GetComponent<Rigidbody>().velocity = vv;
			// b.GetComponent<Rigidbody>().velocity = transform.forward * 5f;
			Destroy(b,5f);
			yield return new WaitForSeconds(2f);
		}
	}

		IEnumerator produceBall2() {

		int i=0;
		while (i<100) {
			i++;
			GameObject b = Instantiate(ball,transform.position,transform.rotation);
			// b.GetComponent<Rigidbody>().AddForce(transform.forward * 2f, ForceMode.Impulse);
			Vector3 vv = GetVelocity(transform.position, new Vector3(transform.position.x, transform.position.y, -3f), 20f);
			b.GetComponent<Rigidbody>().velocity =  vv;
			// b.GetComponent<Rigidbody>().velocity = transform.forward * 5f;
			Destroy(b,5f);
			yield return new WaitForSeconds(2f);
		}
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

}
