using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// ボタンの一覧をキー入力で移動可能に
/// </summary>
public class ButtonGroup : MonoBehaviour {
	public enum Orientation {
		Horizontal,
		Vertical
	}
	[SerializeField]
	private Button[] buttons;

	[SerializeField]
	private Orientation orientation = Orientation.Horizontal;

	public int selected { private set; get; }
	public bool locked { private set; get; }

	// Use this for initialization
	void Start () {
		SetInteractable(true);
		Select(0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	

	public void SetInteractable(bool b) {
		foreach(var button in buttons) {
			button.interactable = b;
		}
	}

	/// <summary>
	/// 指定の要素を選択状態にします。
	/// </summary>
	/// <param name="index"></param>
	public void Select(int index) {
		if(index >= buttons.Length) {
			index = 0;
		}
		if(index < 0) {
			index = buttons.Length - 1;
		}
		EventSystem.current.SetSelectedGameObject(buttons[index].gameObject);
		this.selected = index;
	}

	/// <summary>
	/// 入力状態に応じて選択要素を更新します。
	/// </summary>
	public void InputUpdate() {
		if(locked) {
			return;
		}
		if(this.orientation == Orientation.Horizontal) {
			if(Input.GetKeyDown(KeyCode.LeftArrow)) {
				Select(selected - 1);
			} else if(Input.GetKeyDown(KeyCode.RightArrow)) {
				Select(selected + 1);
			}
		} else if(this.orientation == Orientation.Vertical) {
			if(Input.GetKeyDown(KeyCode.UpArrow)) {
				Select(selected - 1);
			} else if(Input.GetKeyDown(KeyCode.DownArrow)) {
				Select(selected + 1);
			}
		}
		if(Input.GetKeyDown(KeyCode.Space)) {
			buttons[selected].onClick.Invoke();
			this.locked = true;
		}
	}
}
