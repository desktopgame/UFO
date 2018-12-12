using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class StatusEvent {
	public Status source { private set; get; }

	public StatusEvent(Status source) {
		this.source = source;
	}
}

/// <summary>
/// ステータスを表すクラス。
/// </summary>
public class Status : MonoBehaviour {
	[SerializeField]
	private float maxHP = 100;

	/// <summary>
	/// ダメージを受けると呼ばれます。
	/// </summary>
	/// <value></value>
	public IObservable<StatusEvent> onDamage { get { return damage; }}
	private Subject<StatusEvent> damage;

	public float currentHP { private set; get; }

	// Use this for initialization
	void Awake () {
		this.currentHP = maxHP;
		this.damage = new Subject<StatusEvent>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// HPの割合を返します。
	/// </summary>
	/// <returns></returns>
	public float GetHPRatio() {
		return Mathf.Clamp01(currentHP / maxHP);
	}

	/// <summary>
	/// 最大HPを返します。
	/// </summary>
	/// <returns></returns>
	public float GetMaxHP() {
		return maxHP;
	}

	/// <summary>
	/// HPを減らします。
	/// </summary>
	/// <param name="power"></param>
	public void Damage(float power) {
		this.currentHP = Mathf.Max(0f, currentHP - power);
		damage.OnNext(new StatusEvent(this));
	}
}
