using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerControlX : MonoBehaviour, Controls.IPlayerBindsActions
{

    public float rotSpeed = 200f;
    public float speed = 200f;
    public float jumpForce = 50f;

    public float recoilSpeed = 400f;
    public bool isGrounded;
    public bool isFlipped;



    public BodyController body;

    public TextMeshProUGUI bulletTxt;
    private int maxBullets = 50;
    private int currBullets;
    private Rigidbody2D rb;
    private Vector2 inputVel;
    private Controls controls;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        controls = new Controls();
        controls.PlayerBinds.AddCallbacks(this);
        controls.Enable();

        currBullets = maxBullets;
        bulletTxt.text = currBullets + " / " + maxBullets;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(rb.velocity.magnitude > 0.05f){
            isFlipped = Mathf.Sign(rb.velocity.x) == -1;
            body.FlipBody(Mathf.Sign(rb.velocity.x));
        }
    }

    void FixedUpdate()
    {
        rb.AddForce(inputVel * speed);
    }

    

    public void OnMovement(InputAction.CallbackContext context)
    {
        inputVel = context.ReadValue<Vector2>();
        Debug.Log(inputVel);
    }
    

    public void OnJump(InputAction.CallbackContext context)
    {

        if (!context.performed || !isGrounded) { return; }
        Debug.Log("Jump");
        Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 jumpDir = (mouse - new Vector2(transform.position.x, transform.position.y)).normalized;

        rb.AddForce(jumpDir * jumpForce);

    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }
        Debug.Log("Reload!");
        currBullets = maxBullets;
        bulletTxt.text = currBullets + " / " + maxBullets;
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (!context.performed || currBullets <= 0) { return; }
        Debug.Log("Shoot!");
        Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 shootDir = (mouse - new Vector2(transform.position.x, transform.position.y)).normalized;
        rb.AddForce(-shootDir * recoilSpeed);
        currBullets--;
        bulletTxt.text = currBullets + " / " + maxBullets;
    }

    public void OnFlip(InputAction.CallbackContext context) {
        if (!context.performed) {return;}
        
    }



}
