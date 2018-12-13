using UnityEngine;
using System.Collections;
//https://qiita.com/2dgames_jp/items/dc6ba1e9eae05e8f0698

public class Missile : MonoBehaviour {

	[SerializeField, Header("旋回速度")]
    public float _rotSpeed = 3.0f;
	[SerializeField, Header("移動速度")]
    public float _speed = 3.0f;
	[SerializeField, Header("回転を停止してからの初期速度")]
	public float _boost_speed = 10f;
	[SerializeField, Header("回転を停止してからの最大速度")]
	public float _boost_max_speed = 20f;
	[SerializeField, Header("速度上昇にかかる時間")]
	public float _boost_rate = 1f;
	[SerializeField, Header("回転を止めるまでの時間")]
	public float _wait = 3f;
	public GameObject explosion;

	private Rigidbody2D rigid2d;
	private float elapsed;
	private float boost_elapsed;
	private Vector2 oldDir;
	public GameObject target { set; get; }

    /// 移動角度
    float Direction {
        get { return Mathf.Atan2(rigid2d.velocity.y, rigid2d.velocity.x) * Mathf.Rad2Deg; }
    }

	private void Awake() {
		this.rigid2d = GetComponent<Rigidbody2D>();
	}

    /// 角度と速度から移動速度を設定する
    void SetVelocity(float direction, float speed) {
        var vx = Mathf.Cos(Mathf.Deg2Rad * direction) * speed;
        var vy = Mathf.Sin(Mathf.Deg2Rad * direction) * speed;
		this.oldDir = new Vector2(Mathf.Cos(Mathf.Deg2Rad * direction), Mathf.Sin(Mathf.Deg2Rad * direction));
        rigid2d.velocity = new Vector2(vx, vy);
    }

    /// 更新
    void Update () {
		if(elapsed < _wait) {
			this.elapsed += Time.deltaTime;
		} else {
			//数秒たったら回転を止めて突っ込む
			boost_elapsed += Time.deltaTime;
			if(boost_elapsed > _boost_rate) {
				boost_elapsed = 0f;
				_boost_speed += (_boost_max_speed - _boost_speed) / 2f;
			}
        	rigid2d.velocity = oldDir * _boost_speed;
			Debug.Log("boost speed: "+ _boost_speed);
			return;
		}

        // 画像の角度を移動方向に向ける
        var renderer = GetComponent<SpriteRenderer>();
        renderer.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, Direction));

        // ターゲット座標を取得（マウスの座標に向かって移動する）
        //var mousePosition = Input.mousePosition;
        //Vector3 next = Camera.main.ScreenToWorldPoint(mousePosition);
		var next = target.transform.position;
        Vector3 now = transform.position;
        // 目的となる角度を取得する
        var d = next - now;
        var targetAngle = Mathf.Atan2(d.y, d.x) * Mathf.Rad2Deg;
        // 角度差を求める
        var deltaAngle = Mathf.DeltaAngle(Direction, targetAngle);
        var newAngle = Direction;
        if(Mathf.Abs(deltaAngle) < _rotSpeed) {
            // 旋回速度を下回る角度差なので何もしない
        }
        else if(deltaAngle > 0) {
            // 左回り
            newAngle += _rotSpeed;
        }
        else {
            // 右回り
            newAngle -= _rotSpeed;
        }

        // 新しい速度を設定する
        SetVelocity(newAngle, _speed);
    }

	private void OnCollisionEnter2D(Collision2D other) {
		var tag = other.collider.tag;
		if(tag == "Player") {
			ShowExplosion();
			other.collider.GetComponent<Status>().Damage(20);
		} else if(tag == "Block") {
			ShowExplosion();
		} else if(tag == "Enemy") {
			ShowExplosion();
			GameObject.Destroy(other.collider);
		}
	}

	private void ShowExplosion() {
		GameObject.Destroy(gameObject.transform.parent.gameObject);
		var obj = GameObject.Instantiate(explosion);
		obj.transform.position = transform.position;
		GameObject.Destroy(obj, 1f);
	}
}