using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEffect : MonoBehaviour {
	[SerializeField]
	private Vector3 direction;

	[SerializeField]
	private float speed = 5f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += (direction * speed * Time.deltaTime);
	}

	public void SetDirection(Vector3 direction) {
		this.direction = direction;
	}

	public void SetSpeed(float speed) {
		this.speed = speed;
	}
}
