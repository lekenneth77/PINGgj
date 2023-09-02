using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // old

    // public float speed = 10f;
    // private Rigidbody2D rb;

    // private Vector2 inputVel;
    // // Start is called before the first frame update
    // void Start()
    // {
    //     rb = GetComponent<Rigidbody2D>();
    // }

    // // Update is called once per frame
    // void Update()
    // {
    //     inputVel = Vector2.zero;
    //     if(Input.GetKey(KeyCode.W)){
    //         inputVel += (Vector2.up * speed);
    //     }
    //     if(Input.GetKey(KeyCode.A)){
    //         inputVel += (-Vector2.right * speed);
    //     }
    //     if(Input.GetKey(KeyCode.S)){
    //         inputVel += (-Vector2.up * speed);
    //     }
    //     if(Input.GetKey(KeyCode.D)){
    //         inputVel += (Vector2.right * speed);
    //     }

    //     inputVel = inputVel.normalized;
    //     SetRotation();
    // }

    // void FixedUpdate(){
    //     rb.AddForce(inputVel * speed);
        
    // }

    // void SetRotation(){
    //     Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //     // print(mouse);
    // }

}
