using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitByKey : MonoBehaviour {
	[SerializeField]
	private KeyCode key = KeyCode.Space;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(key) && GetComponent<SelectableText>().selected) {
			Application.Quit();
		}
	}
}
