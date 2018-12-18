using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// タイマーの更新イベント。
/// </summary>
public class TimerEvent {
	public TimerUI source { private set; get; }

	public TimerEvent(TimerUI source) {
		this.source = source;
	}
}

/// <summary>
/// タイマー表示のためのスクリプト。
/// </summary>
public class TimerUI : MonoBehaviour {
	[SerializeField]
	private int length = 100;

	[SerializeField]
	private float timeUnit = 1f;

	/// <summary>
	/// 残り時間が減るたびに呼ばれる。
	/// </summary>
	/// <value></value>
	public IObservable<TimerEvent> onTick { get { return tick; }}
	private Subject<TimerEvent> tick;

	/// <summary>
	/// 残り時間が 0 になる瞬間を通知するイベント。
	/// </summary>
	/// <value></value>
	public IObservable<TimerEvent> onElapsed { get { return elapsed; }}
	private Subject<TimerEvent> elapsed;

	/// <summary>
	/// タイマーが停止しているか
	/// </summary>
	/// <value></value>
	public bool isStopped { private set; get;}

	/// <summary>
	/// 残り時間。
	/// </summary>
	/// <value></value>
	public int remine { private set; get;}

	/// <summary>
	/// 残り時間(0~1)。
	/// </summary>
	/// <returns></returns>
	public float parcent { get { return (float)remine / (float)length; }}

	// Use this for initialization
	void Awake () {
		this.remine = length;
		this.tick = new Subject<TimerEvent>();
		this.elapsed = new Subject<TimerEvent>();
	}

	void Start() {
		tick.OnNext(new TimerEvent(this));
		StartCoroutine(ProgressUpdate());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// タイマーを停止
	/// </summary>
	public void Stop() {
		this.isStopped = true;
	}

	private IEnumerator ProgressUpdate() {
		var wait = new WaitForSeconds(timeUnit);
		while(remine > 0) {
			yield return wait;
			yield return new WaitWhile(() => isStopped);
			this.remine -= 1;
			tick.OnNext(new TimerEvent(this));
		}
		elapsed.OnNext(new TimerEvent(this));
	}
}
