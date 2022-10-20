using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anchor : MonoBehaviour
{

    //コンポーネント
    Rigidbody rb;
    SpringJoint springJoint;

    //参照オブジェクト
    public GameObject player;

    //参照スクリプト
    PlayerControler playerControler;

    //アンカーステータス
    public bool hit; //ヒット判定
    public float shotPower; //発射速度
    private Vector3 WaitingPos = new Vector3(0, -100, 0); //非アクティブ時の待機場所



    private void Awake()
    {
        //コンポーネント関連付け
        rb = GetComponent<Rigidbody>();
        springJoint = GetComponent<SpringJoint>();
        playerControler = GameObject.Find("Player").GetComponent<PlayerControler>();

        transform.position = WaitingPos; //アンカーを待機場所へ移動
    }

    void Update()
    {
        //アンカー発射
        rb.AddForce(transform.forward * shotPower, ForceMode.Impulse);
    }

    //アクティブになった時に実行
    private void OnEnable()
    {
        //位置固定解除、回転のみ固定
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        Vector3 cameraDirection = Camera.main.transform.eulerAngles; //カメラの向きを取得
        Quaternion horizontalRotation = Quaternion.AngleAxis(cameraDirection.y, Vector3.up); //カメラの向き(水平方向のみ)取得

        //アンカーをプレイヤーの位置 + カメラの向いてる方向から少し奥へ移動
        if (name == "LeftAnchor")
        {
            rb.transform.position = player.transform.position + (horizontalRotation * new Vector3(-0.3f, 0.9f, 0.4f));
        }
        else if (name == "RightAnchor")
        {
            rb.transform.position = player.transform.position + (horizontalRotation * new Vector3(0.3f, 0.9f, 0.4f));
        }

        //アンカーの発射角度をマウスカーソル地点へ回転
        SetShotDirection();
    }

    //非アクティブになった時に実行
    private void OnDisable()
    {
        Stop();
        transform.position = WaitingPos;
        hit = false;
    }

    //ヒット判定（プレイヤー以外に当たったら止める）
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            Stop();
            hit = true;
        }
    }

    //移動を止める
    void Stop()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    //アンカーをマウスカーソル方向へ向ける
    void SetShotDirection()
    {
        //レイキャスト
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Rayを生成
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) // Rayを投射
        {
            if (hit.collider.CompareTag("Player") == false) // タグを比較
            {
                Vector3 Vec = hit.point - this.transform.position; //ターゲット方向へのベクトルを取得
                Quaternion quaternion = Quaternion.LookRotation(Vec); // ベクトルをオイラー角に変換
                this.transform.rotation = quaternion; //回転
            }
        }
    }
}
