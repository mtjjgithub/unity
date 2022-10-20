using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCast : MonoBehaviour
{
    public float depth;

    // Update is called once per frame
    void Update()
    {
        RayCastSupport();
    }

    public void RayCastSupport()
    {
        //�}�E�X�J�[�\���̈ʒu���擾�A���[���h���W�ɕϊ�
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = depth;
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(mousePos);

        //�^���^�[�Q�b�g���}�E�X�J�[�\���̈ʒu�Ɉړ��i�A���J�[�󌂂��̂��߁j
        transform.position = targetPos;
    }
}
