using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲーム開始時にスコアをリセットする。
/// </summary>
public class ScoreReset : MonoBehaviour {

	// Use this for initialization
	void Start () {
		var len = GameObject.FindGameObjectsWithTag("PickUp").Length;
		ScoreManager.Instance.Reset(len);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
