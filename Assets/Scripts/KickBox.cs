using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickBox : MonoBehaviour
{
    private PlayerControlX player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<PlayerControlX>();
    }

    

    private void OnTriggerEnter2D(Collider2D other) {
        player.isGrounded = true;
    }

    private void OnTriggerExit2D(Collider2D other) {
        player.isGrounded = false;
    }
}
