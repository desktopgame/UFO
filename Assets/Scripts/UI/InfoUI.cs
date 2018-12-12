using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// インフォメーションを表示する
/// </summary>
public class InfoUI : MonoBehaviour {
	[SerializeField]
	private RectTransform start;
	
	[SerializeField]
	private RectTransform end;

	[SerializeField]
	private RectTransform seek;

	public float progress {
		set {
			value = Mathf.Clamp01(value);
			mProgress = value;
			var startPos = start.position;
			var endPos = end.position;
			var inc = ((endPos.x - startPos.x) * value);
			startPos.x += inc;
			seek.position = startPos;
		}
		get { return mProgress; }
	}
	private float mProgress;

	// Use this for initialization
	void Start () {
		progress = 1f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
