using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenMenu : MonoBehaviour
{
    private string _onMouseHoverPath = "event:/sfx/ui/hover";

    public void StartGame()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/sfx/ui/Start", GetComponent<Transform>().position);
        StartCoroutine(WaitToStart());
    }

    public void Credits()
    {
        SceneManager.LoadScene("CreditsScreen");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void OnMouseEnter()
    {
        FMODUnity.RuntimeManager.PlayOneShot(_onMouseHoverPath, GetComponent<Transform>().position);
    }

    IEnumerator WaitToStart()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Scene01");
    }
}