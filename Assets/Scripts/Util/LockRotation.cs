using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 回転の状態を固定します。
/// </summary>
public class LockRotation : MonoBehaviour {
	private Quaternion rotation;
	// Use this for initialization
	void Start () {
		this.rotation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = rotation;
	}
}
