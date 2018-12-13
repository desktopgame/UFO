using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	[SerializeField]
	private float scanDistance = 20f;

	[SerializeField]
	private float scanInterval = 10f;
	[SerializeField]
	private float fireDistance = 5f;

	[SerializeField]
	private GameObject foundIcon;

	[SerializeField]
	private GameObject missile;

	private enum State {
		Search,
		Found,
		Fire,
	}
	private State state;

	// Use this for initialization
	void Start () {
		this.state = State.Search;
	}
	
	// Update is called once per frame
	void Update () {
		SearchUpdate();
	}

	private void SearchUpdate() {
		if(this.state != State.Search) {
			return;
		}
		bool found = false;
		SearchDirection((dir) => {
			var ray = new Ray(transform.position, dir);
			var mask = LayerMask.GetMask("Player", "Block");
			var hit = Physics2D.Raycast(ray.origin, ray.direction, scanDistance, mask);
			if(hit.collider == null) {
				return true;
			}
			if(hit.collider.tag == "Player") {
				found = true;
				Debug.Log("found");
				return false;
			}
			return true;
		});
		//プレイヤーを発見
		if(found) {
			StartCoroutine(SearchToFound());
		}
	}

	private IEnumerator SearchToFound() {
		//!
		this.state = State.Found;
		var obj = GameObject.Instantiate(foundIcon);
		obj.transform.position = transform.position + (Vector3.up * 3);
		yield return new WaitForSeconds(1f);
		GameObject.Destroy(obj);
		//発射
		var player = GameObject.FindGameObjectWithTag("Player");
		var look = (player.transform.position - transform.position).normalized;
		var rotation = Quaternion.LookRotation(look, Vector3.up);
		this.state = State.Fire;
		obj = GameObject.Instantiate(missile);
		obj.transform.position = transform.position + (look * fireDistance);
		obj.GetComponentInChildren<Missile>().target = player;
		yield return new WaitWhile(() => obj != null);
		this.state = State.Search;
	}

	private void OnDrawGizmos() {
		var start = (Vector2)transform.position;
		SearchDirection((dir) => {
			var end = start + (dir * scanDistance);
			Gizmos.DrawLine(start, end);
			return true;
		});
	}

	private void SearchDirection(System.Func<Vector2,bool> act) {
		var rad = 0f;
		while(rad < 360) {
			rad += scanInterval;
			var x = Mathf.Cos(Mathf.Deg2Rad * rad);
			var y = Mathf.Sin(Mathf.Deg2Rad * rad);
			if(!act(new Vector2(x, y))) break;
		}
	}
}
