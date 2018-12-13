using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// ゲームオーバー画面
/// </summary>
public class GameOverUI : MonoBehaviour {
	[SerializeField]
	private Image root;
	[SerializeField]
	private ButtonGroup buttonGroup;

	[SerializeField]
	private float showSeconds = 1.0f;

	private bool inputLock;
	private bool show;

	// Use this for initialization
	void Start () {
		if(buttonGroup == null) {
			this.buttonGroup = GetComponent<ButtonGroup>();
		}
		buttonGroup.SetInteractable(false);
		root.transform.localScale = Vector3.zero;
		this.inputLock = true;
	}
	
	// Update is called once per frame
	void Update () {
		#if UNITY_EDITOR
		if(Input.GetKey(KeyCode.T)) {
			Show();
		}
		#endif
		if(inputLock) { return; }
		buttonGroup.InputUpdate();
	}

	/// <summary>
	/// ダイアログを表示します。
	/// </summary>
	public void Show() {
		if(show) {
			return;
		}
		StartCoroutine(ShowUpdate());
	}

	private IEnumerator ShowUpdate() {
		this.show = true;
		var offset = 0f;
		var separate = 100;
		var segment = showSeconds / separate;
		while(offset < showSeconds) {
			yield return new WaitForSeconds(segment);
			offset += segment;
			root.transform.localScale = Vector3.one * (offset / showSeconds);
		}
		root.transform.localScale = Vector3.one;
		//アニメーションが完了したので入力を許可
		this.inputLock = false;
		buttonGroup.SetInteractable(true);
		buttonGroup.Select(0);
		this.show = false;
	}

	public void ToTitle() {
		FadeUI.instance.StartFade(() =>
        {
        	SceneManager.LoadScene("Title");
        });
	}

	public void Retry() {
		FadeUI.instance.StartFade(() =>
        {
        	SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
	}
}
