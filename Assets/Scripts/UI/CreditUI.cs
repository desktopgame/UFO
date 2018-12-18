using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditUI : MonoBehaviour {
	[SerializeField]
	private ButtonGroup buttonGroup;

	// Use this for initialization
	void Start () {
		if(buttonGroup == null) {
			this.buttonGroup = GetComponent<ButtonGroup>();
		}
		buttonGroup.Select(0);
	}
	
	// Update is called once per frame
	void Update () {
		buttonGroup.InputUpdate();
	}
}
