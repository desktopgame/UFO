using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour {
	[SerializeField]
	private InfoUI info;

	[SerializeField]
	private StageArea area;

	// Use this for initialization
	void Start () {
		if(info == null) {
			this.info = GameObject.Find("InfoCanvas").GetComponent<InfoUI>();
		}
		if(area == null) {
			this.area = GameObject.FindGameObjectWithTag("Stage").GetComponent<StageArea>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		var startX = area.GetLeftTop().transform.position.x;
		var endX = area.GetRightBottom().transform.position.x;
		var progX = transform.position.x;
		var parcent = (progX - startX) / (endX - startX);
		info.progress = parcent;
	}
}
