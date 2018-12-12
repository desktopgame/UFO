using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// タイマーの更新と同時にテキストを更新するレンダラー。
/// </summary>
public class TimerTextRenderer : MonoBehaviour {
	[SerializeField]
	private TimerUI timer;

	[SerializeField]
	private Text text;

	private System.IDisposable onElapsedObserver;
	private System.IDisposable onTickObserver;

	// Use this for initialization
	void Start () {
		if(timer == null) {
			this.timer = GetComponent<TimerUI>();
		}
		if(text == null) {
			this.text = GetComponent<Text>();
		}
		this.onElapsedObserver = timer.onTick.Subscribe((e) => {
			text.text = e.source.remine.ToString();
		});
		this.onTickObserver = timer.onElapsed.Subscribe((e) => {
			text.text = "0";
		});
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnDestroy() {
		onElapsedObserver.Dispose();
		onTickObserver.Dispose();
	}
}
