using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CameraController : MonoBehaviour
{

    public GameObject player;       //Public variable to store a reference to the player game object


    private Vector3 offset;         //Private variable to store the offset distance between the player and camera

    private int shakeStack;
    // Use this for initialization
    void Start()
    {
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        offset = transform.position - player.transform.position;
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        if(shakeStack > 0) { return; }
        // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
        transform.position = player.transform.position + offset;
    }

    public void Shake() {
        StartCoroutine(StartShake());
    }

    private IEnumerator StartShake() {
        shakeStack++;
        iTween.ShakePosition(gameObject,iTween.Hash("x",0.3f,"y",0.3f,"time",0.5f));
        yield return new WaitForSeconds(0.5f);
        shakeStack--;
    }
}
