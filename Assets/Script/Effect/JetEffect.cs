using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetEffect : MonoBehaviour {
	[SerializeField]
	private GameObject particlePrefab;

	[SerializeField]
	private float emitParSeconds = 0.5f;

	[SerializeField]
	private float distance = 5f;

	[SerializeField]
	private float range = 180f;

	[SerializeField]
	private Vector3 scaleMin = new Vector3(0.1f, 0.1f, 0.1f);

	[SerializeField]
	private Vector3 scaleMax = new Vector3(1f, 1f, 1f);

	[SerializeField]
	private int emitMin = 10;

	[SerializeField]
	private int emitMax = 20;

	// Use this for initialization
	void Start () {
		StartCoroutine(EmitParticles());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnDrawGizmos() {
		var offset = 0f;
		var start = (Vector2)transform.position;
		while(offset < range) {
			offset += (range / 10f);
			var r = Mathf.Deg2Rad * offset;
			var dirx = Mathf.Cos(r);
			var diry = Mathf.Sin(r);
			var end = start + (new Vector2(dirx, diry) * distance);
			Gizmos.DrawLine(start, end);
		}
	}

	private IEnumerator EmitParticles() {
		while(true) {
			yield return new WaitForSeconds(emitParSeconds);
			var emits = Random.Range(emitMin, emitMax);
			for(int i=0; i<emits; i++) {
				var r = Random.Range(0f, range) * Mathf.Deg2Rad;
				var dirx = Mathf.Cos(r);
				var diry = Mathf.Sin(r);
				var obj = GameObject.Instantiate(particlePrefab);
				obj.transform.position = transform.position;
				var mf = obj.GetComponent<MoveEffect>();
				mf.SetDirection(new Vector3(dirx, diry, 0));
			}
		}
	}
}
