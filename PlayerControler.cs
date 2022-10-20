using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    //コンポーネント
    Rigidbody rb;
    Animator anim;

    //参照オブジェクト
    public GameObject rightAnchor;
    public GameObject leftAnchor;
    public SpringJoint rightAnchorSJ;
    public SpringJoint leftAnchorSJ;

    //参照スクリプト
    public GroundCheck groundCheck;

    //プレイヤーステータス
    public float speed;
    private float LimitSpeed;
    public float jumpForce;

    public float jetForceY;
    public float jetForceZ;

    public bool isGround;

    public bool rightAnchorReady = true;
    public bool leftAnchorReady = true;


    private void Awake()
    {
        //コンポーネント関連付け
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Run();
    }

    void Update()
    {
        Jump();
        LeftAnchorShot();
        RightAnchorShot();
        TakeUp();
        Jet();
    }

    //走る
    void Run()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        //カメラの向いている方向を取得
        Quaternion horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up); 

        //移動 ベクトル計算
        Vector3 move = horizontalRotation * new Vector3(x, 0, z).normalized;

        //移動 処理
        rb.AddForce(move * speed, ForceMode.Impulse);

        //速度制限
        if (rb.velocity.magnitude > LimitSpeed)
        {
            rb.velocity = new Vector3(rb.velocity.x / 1.1f, rb.velocity.y, rb.velocity.z / 1.1f);
        }

        //移動方向を向く
        if (move.magnitude > 0.5)
        {
            transform.rotation = Quaternion.LookRotation(move, Vector3.up);
        }

        //アニメ再生
        isGround = groundCheck.IsGround(); //接地判定取得
        if (isGround)
        {
            anim.SetFloat("Speed", move.magnitude);
        }

    }

    //ジャンプ
    void Jump()
    {
        isGround = groundCheck.IsGround(); //接地判定取得

        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            rb.AddForce(transform.up * jumpForce,ForceMode.Impulse);
        }

        //Jumpアニメ設定
        anim.SetBool("Jump", isGround);
    }

    //左アンカー発射
    void LeftAnchorShot()
    {
        //Qキーが押されたら右アンカーをアクティブ化
        if (Input.GetKeyDown(KeyCode.Q) && leftAnchorReady)
        {
            leftAnchor.SetActive(true);
            leftAnchorReady = false;
        }
        //Qキーが離れたら右アンカーを非アクティブ化
        else if (Input.GetKeyUp(KeyCode.Q) && !leftAnchorReady)
        {
            leftAnchor.SetActive(false);
            leftAnchorReady = true;
        }
    }

    //右アンカー発射
    void RightAnchorShot()
    {
        //Eキーが押されたら右アンカーをアクティブ化
        if (Input.GetKeyDown(KeyCode.E) && rightAnchorReady)
        {
            rightAnchor.SetActive(true);
            rightAnchorReady = false;
        }
        //Eキーが離れたら右アンカーを非アクティブ化
        else if (Input.GetKeyUp(KeyCode.E) && !rightAnchorReady)
        {
            rightAnchor.SetActive(false);
            rightAnchorReady = true;
        }
    }

    //ジェット
    void Jet()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            rb.AddForce(transform.up * jetForceY,ForceMode.Impulse);
            rb.AddForce(transform.forward * jetForceZ,ForceMode.Impulse);
        }
    }

    //巻き取り
    void TakeUp()
    {
        if (Input.GetMouseButtonDown(1))
        {
            rightAnchorSJ.spring = 300;
            leftAnchorSJ.spring = 300;
            rightAnchorSJ.maxDistance = 0;
            leftAnchorSJ.maxDistance = 0;

            rb.AddForce(transform.up * jetForceY, ForceMode.Impulse);
            rb.AddForce(transform.forward * jetForceZ, ForceMode.Impulse);
        }
    }
}
