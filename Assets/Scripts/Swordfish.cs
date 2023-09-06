using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Swordfish : MonoBehaviour
{

    public Transform player;

    public float chargeSpeed;
    public float chargeTime;
    private Rigidbody2D rb;

    private bool isAttacking;
    private bool startedAttacking;
    private bool isCharging;
    private bool endAttack;

    private Vector2 chargeDir;

    public float t = .2f;

    public float roamSpeed;
    public float roamDist;
    float currentInterval;
    public Vector2 nextDir;

    public bool hitWall;

    public bool dead;

    public SpriteRenderer headSprite;
    private int health = 3;

    private Vector2 stuckPos;
    public AudioSource alertSFX;
    private Vector2 startPos;

    public Sprite[] damageSprites;


    public SpriteRenderer[] limbs;
    public float TimeSinceDamaged;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
        nextDir = -Vector2.right;
        
    }

    // Update is called once per frame
    void Update()
    {

        foreach (SpriteRenderer limb in limbs) {
            limb.color = Color.Lerp(Color.red, Color.white, Mathf.Clamp(TimeSinceDamaged, 0, 1));
        }
        TimeSinceDamaged += Time.deltaTime;
        if (endAttack){
            rb.velocity =  chargeDir * chargeSpeed;
        }
        
        if(hitWall){
            rb.position = stuckPos;
            return;
        }
        if (dead) {return;}
        if (startedAttacking) {return;}
        if(!isAttacking){
            isAttacking = DetectPlayer();
            // Roaming();
        } else{
            StartCoroutine("AttackPlayer");
        }

    }

    
    private void Roaming(){
        if(currentInterval != Mathf.Floor(Time.time / roamDist)){
            currentInterval = Mathf.Floor(Time.time / roamDist);
            nextDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        }
        rb.velocity = (nextDir * roamSpeed);
    }

    private bool DetectPlayer(){
        
        int layerMask = 1 << 7;
        layerMask = ~layerMask;
        Vector2 rayDir = (new Vector2(player.position.x, player.position.y)- new Vector2(transform.position.x, transform.position.y)).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDir, 15f ,layerMask);
        if (hit && hit.collider.CompareTag("Player")) {
            return true;
        } else {
            return false;
        }
        
    }
    float NearestAngle(float a, float b, float t){
        float distanceToMax = 180 - a;
        float distanceFromMin = -180 - b;
        if(Mathf.Abs(b - a) < 180){
            return Mathf.Lerp(a, b, t);
        }else{
            if( a < b){
                return Mathf.Lerp(a + 360, b, t);
            }else{
                return Mathf.Lerp(a, b + 360, t);
            }
        }
    }
    private IEnumerator AttackPlayer(){
        Debug.Log("Started Attacking!");
        startedAttacking = true;
        rb.velocity = Vector3.zero;
        alertSFX.Play();
        while(chargeTime > 0){
            chargeTime -= Time.deltaTime;
           
            Vector2 offset =  new Vector2(transform.position.x, transform.position.y) - new Vector2(player.transform.position.x, player.transform.position.y) ;
            float a = Mathf.Rad2Deg * Mathf.Atan2(offset.y, offset.x);
            
            float nextAngle = NearestAngle(transform.rotation.eulerAngles.z, a, t);
            transform.GetComponent<Rigidbody2D>().MoveRotation(nextAngle);
            if (dead) {yield break;}

            yield return new WaitForEndOfFrame();
        }
        if (dead) {
            yield break;
        }
        isCharging = true;
        chargeDir = (new Vector2(player.position.x, player.position.y)- new Vector2(transform.position.x, transform.position.y)).normalized;
        rb.velocity = chargeDir * chargeSpeed;
        endAttack = true;
    }

    private void OnCollisionEnter2D(Collision2D other) {
       
    }

    private void OnTriggerStay2D(Collider2D other) {
        
        if(!isCharging) return;
        if(other.gameObject.layer != LayerMask.NameToLayer("Enemy") && !other.CompareTag("Player")){
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            endAttack = false;
            isCharging = false;
            stuckPos = rb.position;
            hitWall = true;
        }

        if(other.CompareTag("Player") && endAttack){
            endAttack = false;
            player.transform.parent.GetComponent<PlayerControlX>().controls.Dispose();
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        }
    }

    public void GotShot() {
        if (health <= 0) {return;}
        if (health == 3) {
            isAttacking = true;
        }
        health--;
        headSprite.sprite = damageSprites[health];
        dead = health <= 0;
        if (dead) return;
        TimeSinceDamaged = 0;
    }

}
