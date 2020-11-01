using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    private static FMOD.Studio.EventInstance Music;

    private void Start()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "TitleScreen" || scene.name == "CreditsScreen")
        {
            Music = FMODUnity.RuntimeManager.CreateInstance("event:/Music/menu music");
        }
        else if(scene.name == "Scene01")
        {
            Music = FMODUnity.RuntimeManager.CreateInstance("event:/Music/gameplay music");
        }
        
        Music.start();
        Music.release();
    }

    public void Progress(float ProgressionLevel)
    {
        Music.setParameterByName("Progress", ProgressionLevel);
    }

    private void OnDestroy()
    {
        Music.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
