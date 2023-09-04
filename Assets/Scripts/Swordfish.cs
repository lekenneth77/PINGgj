using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swordfish : MonoBehaviour
{

    public Transform player;

    public float chargeSpeed;
    public float chargeTime;
    private Rigidbody2D rb;

    private bool isAttacking;
    private bool startedAttacking;
    private bool endAttack;

    private Vector2 chargeDir;

    public float t = .2f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(endAttack){
            rb.velocity =  chargeDir * chargeSpeed;
        }
        if (startedAttacking) {return;}
        if(!isAttacking){
            isAttacking = DetectPlayer();
            Roaming();
        } else{
            StartCoroutine("AttackPlayer");
        }
        
        
    }

    
    private void Roaming(){

    }

    private bool DetectPlayer(){
        
        int layerMask = 1 << 7;
        layerMask = ~layerMask;
        Vector2 rayDir = (new Vector2(player.position.x, player.position.y)- new Vector2(transform.position.x, transform.position.y)).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDir, 12.5f ,layerMask);
        if (hit.collider.tag == "Player") {
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
        
        while(chargeTime > 0){
            chargeTime -= Time.deltaTime;
           
            Vector2 offset =  new Vector2(transform.position.x, transform.position.y) - new Vector2(player.transform.position.x, player.transform.position.y) ;
            float a = Mathf.Rad2Deg * Mathf.Atan2(offset.y, offset.x);
            
            float nextAngle = NearestAngle(transform.rotation.eulerAngles.z, a, t);
            transform.GetComponent<Rigidbody2D>().MoveRotation(nextAngle);


            yield return new WaitForEndOfFrame();
        }
        chargeDir = (new Vector2(player.position.x, player.position.y)- new Vector2(transform.position.x, transform.position.y)).normalized;
        rb.velocity = chargeDir * chargeSpeed;
        endAttack = true;
    }

    private void OnCollisionEnter2D(Collision2D other) {
       
    }

}
