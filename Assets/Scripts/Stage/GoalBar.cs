using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// ゴール判定用のスクリプト。
/// </summary>
public class GoalBar : MonoBehaviour {
	public IObservable<bool> onGoal { get { return goal; }}
	private Subject<bool> goal;
	private bool triggered;

	// Use this for initialization
	void Awake () {
		this.goal = new Subject<bool>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if(!triggered) {
			this.triggered = true;
			goal.OnNext(true);
			AudioManager.Instance.PlaySE(AUDIO.SE_GOAL);
		}
	}
}
