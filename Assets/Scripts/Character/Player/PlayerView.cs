using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerView : MonoBehaviour {
	[SerializeField]
	private InfoUI info;

	[SerializeField]
	private TimerUI timer;

	[SerializeField]
	private StageArea area;

	private System.IDisposable observer;

	// Use this for initialization
	void Start () {
		if(info == null) {
			this.info = GameObject.Find("InfoCanvas").GetComponent<InfoUI>();
		}
		if(timer == null) {
			this.timer = GameObject.Find("TimerCanvas").GetComponent<TimerUI>();
		}
		if(area == null) {
			this.area = GameObject.FindGameObjectWithTag("Stage").GetComponent<StageArea>();
		}
		//ゴールしたらバーを右端に
		var goal = GameObject.FindGameObjectWithTag("Goal").GetComponent<GoalBar>();
		this.observer = goal.onGoal.Subscribe((e) => {
			info.progress = 1f;
			timer.Stop();
		});
	}
	
	// Update is called once per frame
	void Update () {
		var startX = area.GetLeftTop().transform.position.x;
		var endX = area.GetRightBottom().transform.position.x;
		var progX = transform.position.x;
		var parcent = (progX - startX) / (endX - startX);
		info.progress = parcent;
	}

	private void OnDestroy() {
		observer.Dispose();
	}
}
