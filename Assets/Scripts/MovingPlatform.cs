using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    private Vector3 startingPos;
    public Transform target;
    private Vector3 otherPos;
    bool goToOther;
    public float movementSpeed = 3f;
    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
        otherPos = target.position;
        goToOther = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (goToOther)
        {
            if (Mathf.Abs(Vector3.Magnitude(otherPos) - Vector3.Magnitude(transform.position)) <= Mathf.Epsilon) {
                goToOther = false;
            }
            else { 
                transform.position = Vector3.MoveTowards(transform.position, otherPos, movementSpeed * Time.deltaTime);
            }
        } else
        {
            if (Mathf.Abs(Vector3.Magnitude(startingPos) - Vector3.Magnitude(transform.position)) <= Mathf.Epsilon) {
                goToOther = true;
            } else { 
                transform.position = Vector3.MoveTowards(transform.position, startingPos, movementSpeed * Time.deltaTime);
            }
        }
    }
}
