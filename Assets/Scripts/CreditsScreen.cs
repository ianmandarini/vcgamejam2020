using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsScreen : MonoBehaviour
{
    //Colocando os caminhos em variáveis senão o FMOD reclama de não tê-los carregado antes
    private string _onMouseHoverPath = "event:/sfx/ui/hover";
    private string _onMouseDownPath = "event:/sfx/ui/click";

    public void CreditsSceneButton()
    {
        SceneManager.LoadScene("TitleScreen");
    }

    public void OnMouseEnter()
    {
        FMODUnity.RuntimeManager.PlayOneShot(_onMouseHoverPath, GetComponent<Transform>().position);
    }

    public void OnMouseDown()
    {
        FMODUnity.RuntimeManager.PlayOneShot(_onMouseDownPath, GetComponent<Transform>().position);
    }
}
