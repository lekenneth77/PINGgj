using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;

    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void FlipBody(float sign) {
        if (sign > 0) {
            //rb.MoveRotation(Quaternion.Euler(0, 0, 0)); 
            rb.SetRotation(Quaternion.Euler(0, 0, transform.rotation.z)); 
        } else {
            //rb.MoveRotation(Quaternion.Euler(0, 180f, 0)); 
            rb.SetRotation(Quaternion.Euler(0, 180f, transform.rotation.z));
        }
        //transform.localScale = new Vector3(sign * Mathf.Abs(transform.localScale.x), 1, 1);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 norm = collision.contacts[0].normal;
        Debug.Log(norm);
        rb.AddForce(norm * 1000f);
    }


}
