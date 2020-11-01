using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static FMOD.Studio.EventInstance Music;

    private void Start()
    {
        Music = FMODUnity.RuntimeManager.CreateInstance("event:/Music/gameplay music");
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
