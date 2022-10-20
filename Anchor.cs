using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anchor : MonoBehaviour
{

    //�R���|�[�l���g
    Rigidbody rb;
    SpringJoint springJoint;

    //�Q�ƃI�u�W�F�N�g
    public GameObject player;

    //�Q�ƃX�N���v�g
    PlayerControler playerControler;

    //�A���J�[�X�e�[�^�X
    public bool hit; //�q�b�g����
    public float shotPower; //���ˑ��x
    private Vector3 WaitingPos = new Vector3(0, -100, 0); //��A�N�e�B�u���̑ҋ@�ꏊ



    private void Awake()
    {
        //�R���|�[�l���g�֘A�t��
        rb = GetComponent<Rigidbody>();
        springJoint = GetComponent<SpringJoint>();
        playerControler = GameObject.Find("Player").GetComponent<PlayerControler>();

        transform.position = WaitingPos; //�A���J�[��ҋ@�ꏊ�ֈړ�
    }

    void Update()
    {
        //�A���J�[����
        rb.AddForce(transform.forward * shotPower, ForceMode.Impulse);
    }

    //�A�N�e�B�u�ɂȂ������Ɏ��s
    private void OnEnable()
    {
        //�ʒu�Œ�����A��]�̂݌Œ�
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        Vector3 cameraDirection = Camera.main.transform.eulerAngles; //�J�����̌������擾
        Quaternion horizontalRotation = Quaternion.AngleAxis(cameraDirection.y, Vector3.up); //�J�����̌���(���������̂�)�擾

        //�A���J�[���v���C���[�̈ʒu + �J�����̌����Ă�������班�����ֈړ�
        if (name == "LeftAnchor")
        {
            rb.transform.position = player.transform.position + (horizontalRotation * new Vector3(-0.3f, 0.9f, 0.4f));
        }
        else if (name == "RightAnchor")
        {
            rb.transform.position = player.transform.position + (horizontalRotation * new Vector3(0.3f, 0.9f, 0.4f));
        }

        //�A���J�[�̔��ˊp�x���}�E�X�J�[�\���n�_�։�]
        SetShotDirection();
    }

    //��A�N�e�B�u�ɂȂ������Ɏ��s
    private void OnDisable()
    {
        Stop();
        transform.position = WaitingPos;
        hit = false;
    }

    //�q�b�g����i�v���C���[�ȊO�ɓ���������~�߂�j
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            Stop();
            hit = true;
        }
    }

    //�ړ����~�߂�
    void Stop()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    //�A���J�[���}�E�X�J�[�\�������֌�����
    void SetShotDirection()
    {
        //���C�L���X�g
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Ray�𐶐�
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) // Ray�𓊎�
        {
            if (hit.collider.CompareTag("Player") == false) // �^�O���r
            {
                Vector3 Vec = hit.point - this.transform.position; //�^�[�Q�b�g�����ւ̃x�N�g�����擾
                Quaternion quaternion = Quaternion.LookRotation(Vec); // �x�N�g�����I�C���[�p�ɕϊ�
                this.transform.rotation = quaternion; //��]
            }
        }
    }
}
