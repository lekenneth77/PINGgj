using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerControlX : MonoBehaviour, Controls.IPlayerBindsActions
{

    public float speed = 10f;
    public float jumpForce = 2000;

    public float recoilSpeed = 1000f;

    public bool isGrounded;
    public bool isFlipped;

    public Rigidbody2D jumpedOffOf;

    public GunController gun;

    public BodyController body;

    public TextMeshProUGUI bulletTxt;

    public AudioSource jumpSFX, shootSFX, moveSFX;
    private int maxBullets = 8;
    private int currBullets;
    public Rigidbody2D rb;
    private Vector2 inputVel;

    public bool onJumpCD;
    private float jumpCooldown = 0.75f;
    private float jumpBuffer = 0;
    public float jumpBufferMax = .5f;

    private bool onShootCD;
    private float shootCD = 0.25f;

    public Image oxyMeter;
    private float maxTime = 300f;
    public float currentTime;

    public Controls controls;

    public Transform spawnpoint;

    // Start is called before the first frame update
    void Start()
    {
        controls = new Controls();
        controls.PlayerBinds.AddCallbacks(this);
        controls.Enable();

        currBullets = maxBullets;
        bulletTxt.text = currBullets + " / " + maxBullets;
        currentTime = maxTime;
        
    }

    // Update is called once per frame
    void Update()
    {
        
        //print("Velo: Mag: " + rb.velocity.magnitude);
        /*
        if(rb.velocity.magnitude > 0.05f){
            isFlipped = Mathf.Sign(rb.velocity.x) == -1;
            body.FlipBody(Mathf.Sign(rb.velocity.x));
        }
        */

        jumpBuffer -= Time.deltaTime;
        currentTime -= Time.deltaTime;
        oxyMeter.fillAmount = currentTime / maxTime;
        if (currentTime <= 0f) {
            currentTime = 1000f;
            controls.Dispose();
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        }
    }

    void FixedUpdate()
    {
        rb.AddForce(inputVel * speed);
    }

    
    public void HitObject(){
        if(!onJumpCD){
            jumpBuffer = jumpBufferMax;

        }
    }
    

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (context.performed && moveSFX) {
            moveSFX.Play();
        } else if (context.canceled && moveSFX) {
            moveSFX.Stop();
        }
        inputVel = context.ReadValue<Vector2>();
        // Debug.Log(inputVel);
    }
    

    public void OnJump(InputAction.CallbackContext context)
    {

        if (!context.performed || onJumpCD || jumpBuffer < 0) { return; }
        Debug.Log("Jump");
        rb.AddForce(inputVel * jumpForce);

        jumpedOffOf?.AddForce(-(inputVel * jumpForce) * 0.25f);

        if (jumpSFX) {
            jumpSFX?.Play();
        }
        StartCoroutine("JumpCooldown");
        jumpBuffer = 0;
    }

    private IEnumerator JumpCooldown() {
        onJumpCD = true;
        yield return new WaitForSeconds(jumpCooldown);
        onJumpCD = false;
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
        if (!context.performed || currBullets <= 0 || onShootCD) { return; }
        // Debug.Log("Shoot!");
        Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 shootDir = (mouse - new Vector2(body.transform.position.x, body.transform.position.y)).normalized;
        rb.AddForce(-shootDir * recoilSpeed);
        currBullets--;
        bulletTxt.text = currBullets + " / " + maxBullets;
        gun.Shoot();
        if (shootSFX) {
            shootSFX?.Play();
        }
        StartCoroutine("ShootCooldown");
    }

    private IEnumerator ShootCooldown()
    {
        onShootCD = true;
        yield return new WaitForSeconds(shootCD);
        onShootCD = false;
    }

    public void OnFlip(InputAction.CallbackContext context) {
        if (!context.performed) {return;}
        
    }





}
