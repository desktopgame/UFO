using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour {
	private PlayerController controller;

	// Use this for initialization
	void Start () {
		this.controller = GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnCollisionEnter2D(Collision2D other) {
		if(controller.freeze) {
			return;
		}
		var status = GetComponent<Status>();
		status.Damage(5f);
		Camera.main.GetComponent<CameraController>().Shake();
		var rigid = GetComponent<Rigidbody2D>();
		rigid.velocity = Vector3.zero;
		AudioManager.Instance.PlaySE(AUDIO.SE_SHOCK);
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if(other.tag == "PickUp") {
			ScoreManager.Instance.Add();
			AudioManager.Instance.PlaySE(AUDIO.SE_ITEMGETSEB);
		}
	}
}
