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
    public float jumpForce = 3000;
    public float recoilSpeed = 1000f;
    public bool isGrounded;
    public bool isFlipped;
    public Rigidbody2D jumpedOffOf;
    public GunController gun;
    public BodyController body;
    public TextMeshProUGUI bulletTxt;
    public AudioSource jumpSFX, shootSFX, moveSFX, hurtSFX;

    private int maxBullets = 1000;
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
    private float maxTime = 3000f;
    public float currentTime;
    public Controls controls;
    public Transform spawnpoint;

    public GameObject jumpArrow;
    public Slider slider;
    public Collider2D oriBox;
    public Collider2D normalJumpBox;
    private Collider2D jumpBox;
    private GameObject jumpHighlighter;
    public Image[] jumpTypes;
    private int prevJump = 0;
    private int whichJump = 0;
    private bool earlyHold; //JANK

    // Start is called before the first frame update
    void Start()
    {
        controls = new Controls();
        controls.PlayerBinds.AddCallbacks(this);
        controls.Enable();

        currBullets = maxBullets;
        bulletTxt.text = currBullets + " / " + maxBullets;
        currentTime = maxTime;

        jumpBox = normalJumpBox;
        jumpHighlighter = jumpBox.transform.GetChild(0).gameObject;
        whichJump = 0;
        jumpBuffer = 0;
        Swordfish.HitPenguin += GotHit;
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
        if (currentTime <= 0f)
        {
            currentTime = 1000f;
            controls.Dispose();
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        }
        if (jumpBuffer < 0f)
        {
            jumpHighlighter.SetActive(false);
        }

        if (Input.GetKey(KeyCode.Space) && whichJump >= 2)
        {
            if (isGrounded)
            {
                jumpHighlighter.SetActive(true);

            } else
            {
                Time.timeScale = 1f;
                jumpHighlighter.SetActive(false);
            }
        }

        if (Input.GetKey(KeyCode.Space) && whichJump >= 5 && isGrounded && earlyHold) {
            earlyHold = false;
            int layerMask = 1 << 6;
            layerMask = ~layerMask;
            rb.velocity = Vector2.zero;
            Collider2D col = Physics2D.OverlapCircle(rb.position, 4f, layerMask);
            rb.position = col.ClosestPoint(rb.position);
            rb.velocity = Vector2.zero;
            Time.timeScale = 0.4f;
        }
        
        if (Input.GetKeyDown(KeyCode.Space) && whichJump >= 5) {
            earlyHold = true; //MEGA DUCTTAPE
        }

        if (Input.GetKeyUp(KeyCode.Space) && whichJump >= 5) {
            Time.timeScale = 1f;
            earlyHold = false;
            jumpHighlighter?.SetActive(false);
            if (whichJump == 5)
            {
                rb.AddForce(inputVel * jumpForce * slider.value);
            } else {
                Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mouseDir = (mouse - new Vector2(rb.position.x, rb.position.y)).normalized;
                //EXTREME SPAGHETTI HOLY SHIT
                if (whichJump == 6) {
                    rb.AddForce(mouseDir * jumpForce * slider.value);
                }
                else if (whichJump == 7) {
                    rb.AddForce(-mouseDir * jumpForce * slider.value);
                }
            }
            Jumped();
        }
        
        if (whichJump == 0 || whichJump == 2 || whichJump == 5) {
            Vector2 offset = inputVel;
            float a = Mathf.Rad2Deg * Mathf.Atan2(-offset.x, offset.y);
            jumpArrow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, a));
        } else if (whichJump == 4 || whichJump == 7) {
            Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 offset = mouse - new Vector2(rb.position.x, rb.position.y);
            float a = Mathf.Rad2Deg * Mathf.Atan2(offset.y, offset.x) + 90f;
            jumpArrow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, a));
        }
        else
        {
            Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 offset = mouse - new Vector2(rb.position.x, rb.position.y);
            float a = Mathf.Rad2Deg * Mathf.Atan2(offset.y, offset.x) - 90f;
            jumpArrow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, a));
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

    public void GotHit()
    {
        currentTime -= 30f;
        hurtSFX.Play();
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
        if (onJumpCD || jumpBuffer < 0) { return; }

        Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseDir = (mouse - new Vector2(rb.position.x, rb.position.y)).normalized;
        switch (whichJump)
        {
            case 0:
                //on press: dir wasd
                Debug.Log("Hey?");
                if (!context.performed) { return; }
                rb.AddForce(inputVel * jumpForce * slider.value);
                Jumped();
                break;
            case 1:
                //on press: dir mouse
                if (!context.performed) { return; }
                rb.AddForce(mouseDir * jumpForce * slider.value);
                Jumped();
                break;
            case 2:
                // hold/release: dir wasd
                jumpHighlighter?.SetActive(true);
                 if (context.canceled) {
                    jumpHighlighter?.SetActive(false);
                    rb.AddForce(inputVel * jumpForce * slider.value);
                    Jumped();
                }
                break;
            case 3:
                // hold/release: dir mouse
                jumpHighlighter?.SetActive(true);
                if (context.canceled) {
                    jumpHighlighter?.SetActive(false);
                    rb.AddForce(mouseDir * jumpForce * slider.value);
                    Jumped();
                }
                break;
            case 4:
                // hold/release: dir inv. mouse
                // hold/release: dir mouse
                jumpHighlighter?.SetActive(true);
                if (context.canceled) {
                    jumpHighlighter?.SetActive(false);
                    rb.AddForce(-mouseDir * jumpForce * slider.value);
                    Jumped();
                }
                break;
            
            default:
                break;
        }
        
        //jumpedOffOf?.AddForce(-(inputVel * jumpForce) * 0.25f);

        
    }

    private IEnumerator JumpCooldown() {
        Debug.Log("Jump");
        onJumpCD = true;
        yield return new WaitForSeconds(jumpCooldown);
        onJumpCD = false;
    }

    private void Jumped()
    {
        if (jumpSFX)
        {
            jumpSFX?.Play();
        }
        StartCoroutine("JumpCooldown");
        jumpBuffer = 0;
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

    public void OnSwapJump(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }
        whichJump = (int)context.ReadValue<float>() - 1;
        Color tmp = jumpTypes[prevJump].color;
        tmp.a = 0.25f;
        jumpTypes[prevJump].color = tmp;
        prevJump = whichJump;

        tmp = jumpTypes[whichJump].color;
        tmp.a = 1f;
        jumpTypes[whichJump].color = tmp;
        if (whichJump >= 5) {
            oriBox.gameObject.SetActive(true);
            normalJumpBox.gameObject.SetActive(false);
            jumpBox = oriBox;
        } else {
            oriBox.gameObject.SetActive(false);
            normalJumpBox.gameObject.SetActive(true);
            jumpBox = normalJumpBox;
        }
        jumpHighlighter = jumpBox.transform.GetChild(0).gameObject;
    }
}
