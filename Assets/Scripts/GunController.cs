using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public Transform parent;
    public float startingAngle;
    public float angleRelativeToBody;

    public GameObject bullet;
    public Transform bulletSP;
    public Transform gunCenter;
    public float t  = .009f;
    public float bulletSpeed = 30f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetRotation();
        SetParentRotation();
    }

    void SetRotation()
    {
        Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 offset = mouse - new Vector2(transform.position.x, transform.position.y) ;
        float a = Mathf.Rad2Deg * Mathf.Atan2(offset.y, offset.x) + startingAngle;
        
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, a));
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

    void SetParentRotation(){
        float theAngle = GetComponentInParent<PlayerControlX>().isFlipped ? -45 : 45;

        float nextAngle = NearestAngle(parent.rotation.eulerAngles.z, transform.rotation.eulerAngles.z + angleRelativeToBody, t);
        parent.GetComponent<Rigidbody2D>().MoveRotation(nextAngle);
        
        //parent.rotation = Quaternion.Slerp(parent.rotation, Quaternion.Euler(new Vector3(0,0,transform.rotation.eulerAngles.z + angleRelativeToBody)), t);
    }

    public void Shoot() {
        //Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 bulletDir = (new Vector2(bulletSP.position.x , bulletSP.position.y) - new Vector2(gunCenter.position.x, gunCenter.position.y)  ).normalized;
        GameObject b = Instantiate(bullet, bulletSP.position, Quaternion.Euler(0,0, transform.rotation.eulerAngles.z), null);
        Rigidbody2D rbullet = b.GetComponent<Rigidbody2D>();
        rbullet.velocity = bulletDir * bulletSpeed;
    }
}
