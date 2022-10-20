using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    public LineRenderer line;
    public Transform anchorPos;
    public Transform playerPos;


    // Start is called before the first frame update
    void Start()
    {
        line.positionCount = 2;
    }

    // Update is called once per frame
    void Update()
    {
        line.SetPosition(0, anchorPos.position);

        if (name == "LeftRope")
        {
            line.SetPosition(1, playerPos.position + new Vector3(-0.18f, 0.9f, 0.1f));
        }
        else if (name == "RightRope")
        {
            line.SetPosition(1, playerPos.position + new Vector3(0.18f, 0.9f, 0.1f));
        }
    }
}
