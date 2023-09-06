using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLevel() {
        SceneManager.LoadSceneAsync("Level 0");
    }

    public void ExitGame() {
        Application.Quit();
    }

    public void LoadMenu() {
        SceneManager.LoadSceneAsync("Title");
    }

}
