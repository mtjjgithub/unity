using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSpringJoint : MonoBehaviour
{

    //�R���|�[�l���g
    Rigidbody rb;
    SpringJoint springJoint;

    //�Q�ƃI�u�W�F�N�g
    public GameObject player;

    //�Q�ƃX�N���v�g
    public Anchor anchorScript;

    //�X�v�����O�X�e�[�^�X
    public float springPower;
    public bool hit; //�q�b�g����
    public bool distanceSet = false; //�v���C���[�ƃA���J�[�̋������Z�b�g�ς݂��ǂ���

    private Vector3 playerPos;
    private Vector3 anchorPos;

    private void Awake()
    {
        //�R���|�[�l���g�֘A�t��
        rb = GetComponent<Rigidbody>();
        springJoint = GetComponent<SpringJoint>();
    }

    void Update()
    {
        playerPos = player.transform.position;
        anchorPos = this.transform.position;

        //�A���J�[�q�b�g���̓v���C���[�̕���������������
        if (hit)
        {
            Vector3 vec = playerPos - anchorPos; //�v���C���[�����ւ̃x�N�g�����擾
            Quaternion quaternion = Quaternion.LookRotation(vec); // �v���C���[�����ւ̃I�C���[�p���擾
            this.transform.rotation = quaternion; //��]
        }
    }

    //��A�N�e�B�u�ɂȂ������Ɏ��s
    private void OnDisable()
    {
        hit = false;

        //�X�v�����O�W���C���g�̏�����
        springJoint.spring = 0;
        springJoint.maxDistance = 0;
        distanceSet = false;
    }

    //�q�b�g����
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            hit = true;

            if (!distanceSet)
            {
                //�v���C���[�ƃA���J�[�̋������擾
                float dis = Vector3.Distance(playerPos, anchorPos);

                //�X�v�����O�W���C���g�̐ݒ�
                springJoint.spring = springPower;
                springJoint.maxDistance = dis;
                distanceSet = true;
            }

        }
    }
}
