using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fire : MonoBehaviour
{
    public Vector3 direction = new Vector3(1, 0, 0);
    public float fireSpeed = 10f;
    public int bruh = 0;
    public PlayerControlX player;
    public bool start;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("ah");
    }

    private IEnumerator ah()
    {
        yield return new WaitForSeconds(2f);
        start = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            transform.Translate(direction * Time.deltaTime * fireSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) {
            bruh++;
            if (bruh == 2)
            {
                player.controls.Dispose();
                SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
            }
        }
    }
}
