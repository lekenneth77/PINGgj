using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StabilizeHead : MonoBehaviour
{
    public PlayerControlX player;
    private HingeJoint2D hj;
    // Start is called before the first frame update
    void Start()
    {
        hj = GetComponent<HingeJoint2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // JointAngleLimits2D limits = hj.limits;
        // if(player.isFlipped){
        //     limits.max = -Mathf.Abs(hj.limits.max);
        //     limits.min = Mathf.Abs(hj.limits.max);
        // } else {
        //     limits.min = -Mathf.Abs(hj.limits.max);
        //     limits.max = Mathf.Abs(hj.limits.max);
        // }
        // hj.limits = limits;
    }
}
