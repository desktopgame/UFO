using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// ゲーム結果を表示するUI
/// </summary>
public class ResultUI : MonoBehaviour {
	[SerializeField]
	private Image root;

	[SerializeField]
	private Text remineItemText;

	[SerializeField]
	private Text damageText;

	[SerializeField]
	private Image[] stars;

	[SerializeField]
	private Button[] buttons;

	[SerializeField]
	private float showSeconds = 1.0f;

	private bool inputLock;
	private bool show;
	private int currentSelect;

	// Use this for initialization
	void Start () {
		root.transform.localScale = Vector3.zero;
		remineItemText.text = "";
		damageText.text = "";
		this.inputLock = true;
		ShowStar(0);
		ButttonEnable(false);
	}

	private void ShowStar(int endToIndex) {
		//全て見えないように
		for(int i=0; i<stars.Length; i++) {
			var star = stars[i];
			star.color = Color.white * 0f;
		}
		//指定のスターまでを見えるように
		for(int i=0; i<endToIndex; i++) {
			var star = stars[i];
			star.color = Color.white;
		}
	}
	
	private void ButttonEnable(bool b) {
		foreach(var button in buttons) {
			button.interactable = b;
		}
	}

	private void SelectButton(int index) {
		if(index >= buttons.Length) {
			index = 0;
		}
		if(index < 0) {
			index = buttons.Length - 1;
		}
		Debug.Log("select " + index);
		EventSystem.current.SetSelectedGameObject(buttons[index].gameObject);
		this.currentSelect = index;
	}

	// Update is called once per frame
	void Update () {
		#if UNITY_EDITOR
		if(Input.GetKey(KeyCode.Z)) {
			Show();
		}
		#endif
		if(inputLock) { return; }
		if(Input.GetKeyDown(KeyCode.LeftArrow)) {
			SelectButton(currentSelect - 1);
		} else if(Input.GetKeyDown(KeyCode.RightArrow)) {
			SelectButton(currentSelect + 1);
		}
		if(Input.GetKeyDown(KeyCode.Space)) {
			buttons[currentSelect].onClick.Invoke();
		}
	}

	/// <summary>
	/// ダイアログを表示します。
	/// </summary>
	public void Show() {
		if(show) {
			return;
		}
		this.show = true;
		StartCoroutine(ShowUpdate());
	}

	private IEnumerator ShowUpdate() {
		//ダイアログを大きくする
		var offset = 0f;
		var separate = 100;
		var segment = showSeconds / separate;
		while(offset < showSeconds) {
			yield return new WaitForSeconds(segment);
			offset += segment;
			var parcent = Mathf.Clamp01(offset / showSeconds);
			root.transform.localScale = Vector3.one * parcent;
		}
		root.transform.localScale = Vector3.one;
		//取れなかった数
		yield return new WaitForSeconds(1f);
		remineItemText.text = (ScoreManager.Instance.maxValue - ScoreManager.Instance.currentValue).ToString();
		AudioManager.Instance.PlaySE(AUDIO.SE_EVAL_OTHRES);
		//ダメージ量を表示
		yield return new WaitForSeconds(1f);
		var player = GameObject.FindGameObjectWithTag("Player");
		var playerStatus = player.GetComponent<Status>();
		damageText.text = (playerStatus.GetMaxHP() - playerStatus.currentHP).ToString();
		AudioManager.Instance.PlaySE(AUDIO.SE_EVAL_OTHRES);
		//総合評価を表示
		yield return new WaitForSeconds(1f);
		AudioManager.Instance.PlaySE(AUDIO.SE_EVAL_RANK);
		ShowStar(CalcRank());
		//アニメーションが完了したので入力を許可
		this.inputLock = false;
		ButttonEnable(true);
		SelectButton(0);
	}

	private int CalcRank() {
		//減点方式
		var ret = 3;
		var player = GameObject.FindGameObjectWithTag("Player");
		var playerStatus = player.GetComponent<Status>();
		//HPが半分以下なら-1
		if(playerStatus.currentHP < playerStatus.GetMaxHP() / 2) {
			ret--;
		//少しでもダメージを受けてるなら-1
		} else if(Mathf.Abs(playerStatus.currentHP - playerStatus.GetMaxHP()) > 0.1f) {
			ret--;
		}
		//半分以上取れていない
		var remines = (ScoreManager.Instance.maxValue - ScoreManager.Instance.currentValue);
		if(remines >= ScoreManager.Instance.maxValue / 2) {
			ret-=2;
		//取れていないものが一つでもあるなら-1
		} else if(remines > 0) {
			ret--;
		}
		//最低でも一点
		if(ret <= 0) {
			ret = 1;
		}
		return ret;
	}
}
