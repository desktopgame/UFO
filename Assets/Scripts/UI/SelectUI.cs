using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct SelectRow {
	public GameObject[] list;
}
/// <summary>
/// 選択画面。
/// </summary>
public class SelectUI : MonoBehaviour {
	[SerializeField]
	private SelectRow[]table;

	[SerializeField]
	private int rowCount = 2;

	[SerializeField]
	private int colCount = 2;

	private int selRow;
	private int selCol;
	private ISelectable target;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.LeftArrow)) {
			SelectLeft();
		} else if(Input.GetKeyDown(KeyCode.RightArrow)) {
			SelectRight();
		}
		if(Input.GetKeyDown(KeyCode.UpArrow)) {
			SelectUp();
		} else if(Input.GetKeyDown(KeyCode.DownArrow)) {
			SelectDown();
		}
		Select();
	}

	private void SelectLeft() {
		selCol--;
		if(selCol < 0) {
			selCol = colCount - 1;
		}
	}

	private void SelectRight() {
		selCol++;
		if(selCol >= colCount) {
			selCol = 0;
		}
	}

	private void SelectUp() {
		selRow--;
		if(selRow < 0) {
			selRow = rowCount - 1;
		}
	}

	private void SelectDown() {
		selRow++;
		if(selRow >= rowCount) {
			selRow = 0;
		}
	}

	private void Select() {
		var newTarget = table[selRow].list[selCol].GetComponent<ISelectable>();
		if(target == newTarget) {
			return;
		}
		if(target != null) {
			target.OnLostFocus();
		}
		newTarget.OnFocus();
		this.target = newTarget;
	}
}
