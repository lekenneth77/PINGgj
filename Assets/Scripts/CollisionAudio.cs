using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAudio : MonoBehaviour
{
    public AudioSource aud;
    private Rigidbody2D rb;
    private bool onCD;
    private float cooldown = 0.5f;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        
        if(rb.velocity.magnitude > .2 && !onCD){
            aud.Play();
            StartCoroutine("AudioCooldown");
        }
    }

    private IEnumerator AudioCooldown() {
        onCD = true;
        yield return new WaitForSeconds(cooldown);
        onCD = false;
    }
}
