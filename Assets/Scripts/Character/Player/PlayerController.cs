﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UniRx;
using System.Timers;
using System;

public class PlayerController : MonoBehaviour {
    public float speed;
    public float brakeWait = 1f;


    private Rigidbody2D rb2d;
    private int count;
    private System.IDisposable observer;
    public bool freeze {
        set { mFreeze = value;
            if(value) {
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                Enemy.StopAll();
                Missile.DestroyAll();
            }
        }
        get{ return mFreeze;}
    }
    private bool mFreeze;

    private bool brakeTriggered;
    private float brakeElapsed;

    // Use this for initialization
    void Start() {
        rb2d = GetComponent<Rigidbody2D>();
        count = 0;
        //ゴールしたらフリーズ
		var goal = GameObject.FindGameObjectWithTag("Goal").GetComponent<GoalBar>();
        goal.onGoal.Subscribe((e) => {
            this.freeze = true;
            rb2d.velocity = rb2d.velocity / 2f;
        });
    }

    void FixedUpdate() {
        if(freeze) {
            return;
        }
        if(brakeTriggered) {
            if((brakeElapsed += Time.deltaTime) > brakeWait) {
                brakeTriggered = false;
            }
            return;
        }
        //ブレーキ
        if(Input.GetKey(KeyCode.LeftShift) ||
           Input.GetKey(KeyCode.RightShift)) {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            this.brakeElapsed = 0f;
            this.brakeTriggered = true;
            return;
        }
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        rb2d.AddForce(movement * speed);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(freeze) {
            return;
        }
        if (other.gameObject.CompareTag("PickUp")) {
            other.gameObject.SetActive(false);
            count = count + 1;
        }
    }
}
