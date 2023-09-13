using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickBox : MonoBehaviour
{
    public PlayerControlX player;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    

    private void OnTriggerStay2D(Collider2D other) {
        player.isGrounded = true;
        player.HitObject();

        if (other.GetComponent<Rigidbody2D>()) {
            if (!player.jumpedOffOf) {
                player.jumpedOffOf = other.GetComponent<Rigidbody2D>();
            }
        } else
        {
            player.jumpedOffOf = null;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        player.isGrounded = false;
    }
}
