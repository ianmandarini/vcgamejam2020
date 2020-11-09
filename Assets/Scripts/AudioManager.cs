using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static FMOD.Studio.EventInstance _staticMusicInstance;
    public static DanceSystem _staticDanceSystem;

    private void Awake()
    {
        _staticDanceSystem = GetComponent<DanceSystem>();

        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "TitleScreen")
        {
            _staticMusicInstance = FMODUnity.RuntimeManager.CreateInstance("event:/Music/menu music");
        }
        else if(scene.name == "Scene01")
        {
            _staticMusicInstance = FMODUnity.RuntimeManager.CreateInstance("event:/music/gameplay music");    
        }

        _staticMusicInstance.start();
        _staticDanceSystem.AssignBeatEvent(_staticMusicInstance);
        _staticMusicInstance.release();
    }

    private void FixedUpdate()
    {
        //Debug.Log(DanceSystem.marker);
    }

    private void OnDestroy()
    {
        _staticMusicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
