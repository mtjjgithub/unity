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
        //マウスカーソルの位置を取得、ワールド座標に変換
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = depth;
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(mousePos);

        //疑似ターゲットをマウスカーソルの位置に移動（アンカー空撃ちのため）
        transform.position = targetPos;
    }
}
