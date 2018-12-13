using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// スコア情報の管理。
/// </summary>
/// <typeparam name="ScoreManager"></typeparam>
public class ScoreManager : SingletonMonoBehaviour<ScoreManager> {
	public int maxValue { private set; get; }
	public int currentValue { private set; get; }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// スコアをリセットします。
	/// </summary>
	/// <param name="maxValue"></param>
	public void Reset(int maxValue) {
		this.maxValue = maxValue;
		this.currentValue = 0;
	}
}
