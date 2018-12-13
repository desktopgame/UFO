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

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter2D(Collider2D other) {
		goal.OnNext(true);
	}
}
