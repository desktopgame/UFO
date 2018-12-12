using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// ステータスの状態を表示するクラス。
/// </summary>
public class StatusUI : MonoBehaviour {
	[SerializeField]
	private Status status;

	[SerializeField]
	private Slider slider;

	private System.IDisposable observer;

	// Use this for initialization
	void Start () {
		if(status == null) {
			this.status = GetComponent<Status>();
		}
		if(slider == null) {
			this.slider = GetComponent<Slider>();
		}
		this.observer = status.onDamage.Subscribe((e) => {
			slider.value = e.source.GetHPRatio();
		});
		slider.value = 1;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
