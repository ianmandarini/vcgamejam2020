using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Scene01");
    }

    public void Credits()
    {
        SceneManager.LoadScene("CreditsScreen");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
