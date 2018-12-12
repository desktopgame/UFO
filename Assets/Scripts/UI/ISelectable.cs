using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 選択可能な要素。
/// </summary>
public interface ISelectable {
	void OnFocus();

	void OnLostFocus();
}
