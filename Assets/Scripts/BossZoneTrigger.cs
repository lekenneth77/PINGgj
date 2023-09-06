using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BossZoneTrigger : MonoBehaviour
{
    public CinemachineVirtualCamera cam;
    public GameObject pipe;
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet")) { 
            Destroy(collision.gameObject); 
            return; 
        }
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player")) { return; }

        cam.m_Lens.OrthographicSize = 15f;
        cam.transform.GetChild(0).gameObject.SetActive(false);
        pipe.SetActive(true);
        gameObject.SetActive(false);
    }
}
