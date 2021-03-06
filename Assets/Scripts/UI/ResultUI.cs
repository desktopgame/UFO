﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
	private ButtonGroup buttonGroup;

	[SerializeField]
	private float showSeconds = 1.0f;

	[SerializeField]
	private string nextSceneName = "Title";

	private bool inputLock;
	private bool show;

	// Use this for initialization
	void Start () {
		if(buttonGroup == null) {
			this.buttonGroup = GetComponent<ButtonGroup>();
		}
		root.transform.localScale = Vector3.zero;
		remineItemText.text = "";
		damageText.text = "";
		buttonGroup.SetInteractable(false);
		this.inputLock = true;
		ShowStar(0);
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
	
	// Update is called once per frame
	void Update () {
		#if UNITY_EDITOR
		if(Input.GetKey(KeyCode.Z)) {
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
		this.show = true;
		StartCoroutine(ShowUpdate());
	}

	public void ToTitle() {
		FadeUI.instance.StartFade(() =>
        {
        	    SceneManager.LoadScene("Title");
        });
	}

	public void ToSelect() {
		FadeUI.instance.StartFade(() =>
        {
        	    SceneManager.LoadScene("Select");
        });
	}

	public void ToNext() {
		FadeUI.instance.StartFade(() =>
        {
        	    SceneManager.LoadScene(nextSceneName);
        });
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
		buttonGroup.SetInteractable(true);
		buttonGroup.Select(0);
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
