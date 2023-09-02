using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControlX : MonoBehaviour, Controls.IPlayerBindsActions
{

    public float rotSpeed = 200f;
    public float speed = 200f;
    public float jumpForce = 50f;
    public bool isGrounded;
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
    }

    // Update is called once per frame
    void Update()
    {
        //SetRotation();
        
        if(Input.GetKey(KeyCode.Q)){ 
            transform.Rotate(new Vector3(0, 0, rotSpeed * Time.deltaTime));
           //transform.rotation = Quaternion.Euler(0,0,transform.rotation.z + 2);
        } else if(Input.GetKey(KeyCode.E)){
            transform.Rotate(new Vector3(0, 0, -rotSpeed * Time.deltaTime));
           //transform.rotation = Quaternion.Euler(0,0,transform.rotation.z - 2);
        }
    }

    void FixedUpdate()
    {
        rb.AddForce(inputVel * speed);
    }

    void SetRotation()
    {
        // Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Vector2 offset = mouse - new Vector2(transform.position.x, transform.position.y) ;
        // float a = Mathf.Rad2Deg * Mathf.Atan2(offset.y, offset.x);
        
        // transform.rotation = Quaternion.Euler(new Vector3(0, 0, a));
        // print(mouse);
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        inputVel = context.ReadValue<Vector2>();
        Debug.Log(inputVel);
    }
    

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed || !isGrounded) { return; }
        
        rb.AddForce(Mathf.Sign(transform.lossyScale.y) * transform.up * jumpForce);

    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }
        Debug.Log("Reload!");
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }
        Debug.Log("Shoot!");
    }

    public void OnFlip(InputAction.CallbackContext context) {
        if (!context.performed) {return;}
        
        transform.localScale = new Vector3(1, -transform.localScale.y, 1);
    }



}
