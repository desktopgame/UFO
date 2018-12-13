using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// フェード表現のためのスクリプト。
/// </summary>
public class FadeUI : CachedMonoBehaviour<FadeUI> {
    [SerializeField]
    private Image image;

    [SerializeField]
    private float fadeSeconds = 1f;

    public bool fadeNow { private set; get; }

	// Use this for initialization
	void Start () {
		if(this.image == null)
        {
            this.image = GetComponentInChildren<Image>();
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartFade(System.Action doInBackground)
    {
        if(fadeNow) {
            return;
        }
        StartCoroutine(FadeWithAction(doInBackground));
    }

    private IEnumerator FadeWithAction(System.Action doInBackground)
    {
        this.fadeNow = true;
        yield return FadeUpdate(fadeSeconds, (e) => e);
        doInBackground();
        yield return FadeUpdate(fadeSeconds, (e) => 1 - e);
        this.fadeNow = false;
    }

    private IEnumerator FadeUpdate(float seconds, System.Func<float,float> calcAlpha)
    {
        var offset = 0f;
        var separate = 100f;
        var segment = seconds / separate;
        var baseColor = image.color;
        while(offset < seconds)
        {
            yield return new WaitForSeconds(segment);
            offset += segment;
            var parcent = offset / seconds;
            var alpha = calcAlpha(parcent);
            baseColor.a = alpha;
            image.color = baseColor;
        }
        baseColor.a = calcAlpha(1f);
        image.color = baseColor;
    }
}
