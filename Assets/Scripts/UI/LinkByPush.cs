using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ボタンが押されたら指定のURLへジャンプ
/// </summary>
public class LinkByPush : MonoBehaviour {
	[SerializeField]
	private string url;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void JumpURL() {
		Application.OpenURL(url);
	}
}
