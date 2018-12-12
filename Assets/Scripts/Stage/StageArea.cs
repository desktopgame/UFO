using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageArea : MonoBehaviour {
	private GameObject leftTop;
	private GameObject rightBottom;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public GameObject GetLeftTop() {
		if(leftTop == null) {
			this.leftTop = transform.Find("LeftTop").gameObject;
		}
		return leftTop;
	}

	public GameObject GetRightBottom() {
		if(rightBottom == null) {
			this.rightBottom = transform.Find("RightBottom").gameObject;
		}
		return rightBottom;
	}
}
