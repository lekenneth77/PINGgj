using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void FlipBody(float sign) {
        transform.localScale = new Vector3(sign * Mathf.Abs(transform.localScale.x), 1, 1);
    }
}
