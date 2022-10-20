using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    //�R���|�[�l���g
    Rigidbody rb;
    Animator anim;

    //�Q�ƃI�u�W�F�N�g
    public GameObject rightAnchor;
    public GameObject leftAnchor;
    public SpringJoint rightAnchorSJ;
    public SpringJoint leftAnchorSJ;

    //�Q�ƃX�N���v�g
    public GroundCheck groundCheck;

    //�v���C���[�X�e�[�^�X
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
        //�R���|�[�l���g�֘A�t��
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

    //����
    void Run()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        //�J�����̌����Ă���������擾
        Quaternion horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up); 

        //�ړ� �x�N�g���v�Z
        Vector3 move = horizontalRotation * new Vector3(x, 0, z).normalized;

        //�ړ� ����
        rb.AddForce(move * speed, ForceMode.Impulse);

        //���x����
        if (rb.velocity.magnitude > LimitSpeed)
        {
            rb.velocity = new Vector3(rb.velocity.x / 1.1f, rb.velocity.y, rb.velocity.z / 1.1f);
        }

        //�ړ�����������
        if (move.magnitude > 0.5)
        {
            transform.rotation = Quaternion.LookRotation(move, Vector3.up);
        }

        //�A�j���Đ�
        isGround = groundCheck.IsGround(); //�ڒn����擾
        if (isGround)
        {
            anim.SetFloat("Speed", move.magnitude);
        }

    }

    //�W�����v
    void Jump()
    {
        isGround = groundCheck.IsGround(); //�ڒn����擾

        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            rb.AddForce(transform.up * jumpForce,ForceMode.Impulse);
        }

        //Jump�A�j���ݒ�
        anim.SetBool("Jump", isGround);
    }

    //���A���J�[����
    void LeftAnchorShot()
    {
        //Q�L�[�������ꂽ��E�A���J�[���A�N�e�B�u��
        if (Input.GetKeyDown(KeyCode.Q) && leftAnchorReady)
        {
            leftAnchor.SetActive(true);
            leftAnchorReady = false;
        }
        //Q�L�[�����ꂽ��E�A���J�[���A�N�e�B�u��
        else if (Input.GetKeyUp(KeyCode.Q) && !leftAnchorReady)
        {
            leftAnchor.SetActive(false);
            leftAnchorReady = true;
        }
    }

    //�E�A���J�[����
    void RightAnchorShot()
    {
        //E�L�[�������ꂽ��E�A���J�[���A�N�e�B�u��
        if (Input.GetKeyDown(KeyCode.E) && rightAnchorReady)
        {
            rightAnchor.SetActive(true);
            rightAnchorReady = false;
        }
        //E�L�[�����ꂽ��E�A���J�[���A�N�e�B�u��
        else if (Input.GetKeyUp(KeyCode.E) && !rightAnchorReady)
        {
            rightAnchor.SetActive(false);
            rightAnchorReady = true;
        }
    }

    //�W�F�b�g
    void Jet()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            rb.AddForce(transform.up * jetForceY,ForceMode.Impulse);
            rb.AddForce(transform.forward * jetForceZ,ForceMode.Impulse);
        }
    }

    //�������
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
