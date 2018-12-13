using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// スコア情報の管理。
/// </summary>
/// <typeparam name="ScoreManager"></typeparam>
public class ScoreManager : SingletonMonoBehaviour<ScoreManager> {
	public IObservable<bool> onChanged { get { return changed; }}
	private Subject<bool> changed;
	public int maxValue { private set; get; }
	public int currentValue { private set; get; }

	// Use this for initialization
	void Start () {
		this.changed = new Subject<bool>();
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

	/// <summary>
	/// スコアに1追加。
	/// </summary>
	public void Add() {
		currentValue += 1;
		changed.OnNext(true);
	}
}
