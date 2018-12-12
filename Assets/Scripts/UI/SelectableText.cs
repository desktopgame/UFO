using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectableText : MonoBehaviour, ISelectable {
	[SerializeField]
	private Text text;

	[SerializeField]
	private Color selectColor;

	private Color color;

	// Use this for initialization
	void Start () {
		if(text == null) {
			this.text = GetComponentInChildren<Text>();
		}
		this.color = text.color;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void OnFocus() {
		text.color = selectColor;
	}

	public void OnLostFocus() {
		text.color = color;
	}
}
