using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public Transform parent;
    public float startingAngle;
    public float angleRelativeToBody;

    public float t  = .009f;
    
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

    void SetParentRotation(){
        float theAngle = GetComponentInParent<PlayerControlX>().isFlipped ? -45 : 45;
        parent.rotation = Quaternion.Slerp(parent.rotation, Quaternion.Euler(new Vector3(0,0,transform.rotation.eulerAngles.z + angleRelativeToBody)), t);
    }
}
