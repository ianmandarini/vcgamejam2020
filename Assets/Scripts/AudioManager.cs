using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    private static FMOD.Studio.EventInstance instance;
    private DanceSystem danceSystem;

    private void Start()
    {
        danceSystem = GetComponent<DanceSystem>();

        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "TitleScreen")
        {
            instance = FMODUnity.RuntimeManager.CreateInstance("event:/Music/menu music");
        }
        else if(scene.name == "Scene01")
        {
            instance = FMODUnity.RuntimeManager.CreateInstance("event:/music/gameplay music");    
        }

        instance.start();
        danceSystem.AssignBeatEvent(instance);
        instance.release();
    }

    private void FixedUpdate()
    {
        Debug.Log(DanceSystem.marker);
    }

    private void OnDestroy()
    {
        instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
