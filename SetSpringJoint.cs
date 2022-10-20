using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSpringJoint : MonoBehaviour
{

    //コンポーネント
    Rigidbody rb;
    SpringJoint springJoint;

    //参照オブジェクト
    public GameObject player;

    //参照スクリプト
    public Anchor anchorScript;

    //スプリングステータス
    public float springPower;
    public bool hit; //ヒット判定
    public bool distanceSet = false; //プレイヤーとアンカーの距離をセット済みかどうか

    private Vector3 playerPos;
    private Vector3 anchorPos;

    private void Awake()
    {
        //コンポーネント関連付け
        rb = GetComponent<Rigidbody>();
        springJoint = GetComponent<SpringJoint>();
    }

    void Update()
    {
        playerPos = player.transform.position;
        anchorPos = this.transform.position;

        //アンカーヒット中はプレイヤーの方向を向き続ける
        if (hit)
        {
            Vector3 vec = playerPos - anchorPos; //プレイヤー方向へのベクトルを取得
            Quaternion quaternion = Quaternion.LookRotation(vec); // プレイヤー方向へのオイラー角を取得
            this.transform.rotation = quaternion; //回転
        }
    }

    //非アクティブになった時に実行
    private void OnDisable()
    {
        hit = false;

        //スプリングジョイントの初期化
        springJoint.spring = 0;
        springJoint.maxDistance = 0;
        distanceSet = false;
    }

    //ヒット判定
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            hit = true;

            if (!distanceSet)
            {
                //プレイヤーとアンカーの距離を取得
                float dis = Vector3.Distance(playerPos, anchorPos);

                //スプリングジョイントの設定
                springJoint.spring = springPower;
                springJoint.maxDistance = dis;
                distanceSet = true;
            }

        }
    }
}
