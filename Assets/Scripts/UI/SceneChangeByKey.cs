using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeByKey : MonoBehaviour {
	[SerializeField]
	private KeyCode key = KeyCode.Space;
	
	[SerializeField]
	private string sceneName;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(key) && GetComponent<SelectableText>().selected) {
			FadeUI.instance.StartFade(() =>
        	{
        	    SceneManager.LoadScene(sceneName);
        	});
		}
	}
}
