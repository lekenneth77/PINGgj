using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // public float bulletVel = 30f;
    // private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        // rb = GetComponent<Rigidbody2D>();
        // rb.velocity = transform.right * bulletVel;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other) {

        if(other.gameObject.layer != LayerMask.NameToLayer("Player")){
            Destroy(gameObject);
        }
    }
}
