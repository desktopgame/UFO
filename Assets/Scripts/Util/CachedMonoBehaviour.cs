using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// シーンをまたいで存在し続けるオブジェクトの基底クラス。
/// </summary>
/// <typeparam name="T"></typeparam>
public class CachedMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour {
    /// <summary>
    /// 唯一のインスタンス。
    /// </summary>
    public static T instance { private set; get; }

    private void Awake()
    {
        if(instance == null)
        {
            instance = FindObjectOfType<T>();
            DontDestroyOnLoad(gameObject);
            PostAwake();
        } else
        {
            Destroy(gameObject);
        }
    }
    
    /// <summary>
    /// インスタンス生成直後に呼ばれます。
    /// </summary>
    protected virtual void PostAwake()
    {

    }
}
