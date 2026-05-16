using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool _isGameOver = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver)
        {
            //Reload the current scene
            SceneManager.LoadScene(1); //Set it in File -> Build Settings -> Add Open Scenes and use the index of the scene
        }
    }

    public void GameOver()
    {
        _isGameOver = true;
    }
}
