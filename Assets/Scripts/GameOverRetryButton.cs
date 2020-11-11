using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverRetryButton : MonoBehaviour
{
    private Scene _activeScene;

    void Start()
    {
        _activeScene = SceneManager.GetActiveScene();    
    }

    void FixedUpdate()
    {
        if (Input.GetButtonDown("Jump")) //Jump = Space Button
        {
            SceneManager.LoadScene(_activeScene.buildIndex);
        }
    }
}
