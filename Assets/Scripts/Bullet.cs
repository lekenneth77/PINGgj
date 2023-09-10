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
        //this.GetComponent<SpriteRenderer>().color = new Color(Random.Range(0, 1f), Random.Range(0, 1f), 1);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            Destroy(gameObject);
        }
        if (other.gameObject.GetComponent<Swordfish>())
        {
            Swordfish fish = other.gameObject.GetComponent<Swordfish>();
            fish.GotShot();
        }
        // hiiiiiii
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Target"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
