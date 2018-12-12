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
	public bool selected { private set; get; }

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
		this.selected = true;
	}

	public void OnLostFocus() {
		text.color = color;
		this.selected = false;
	}
}
