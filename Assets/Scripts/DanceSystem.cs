using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class DanceSystem : MonoBehaviour
{
    [StructLayout(LayoutKind.Sequential)]
    class TimelineInfo
    {
        public int currentMusicBeat = 0;
        public FMOD.StringWrapper lastMarker = new FMOD.StringWrapper();
    }

    TimelineInfo timelineInfo;
    GCHandle timelineHandle;

    FMOD.Studio.EVENT_CALLBACK beatCallback;

    public static int beat;
    public static string marker;

    public void AssignBeatEvent(FMOD.Studio.EventInstance instance)
    {
        timelineInfo = new TimelineInfo();
        timelineHandle = GCHandle.Alloc(timelineInfo, GCHandleType.Pinned);
        beatCallback = new FMOD.Studio.EVENT_CALLBACK(BeatEventCallback);
        instance.setUserData(GCHandle.ToIntPtr(timelineHandle));
        instance.setCallback(beatCallback, FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT | FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER);
    }

    public void StopAndClear(FMOD.Studio.EventInstance instance)
    {
        instance.setUserData(IntPtr.Zero);
        instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        instance.release();
        timelineHandle.Free();
    }

    [AOT.MonoPInvokeCallback(typeof(FMOD.Studio.EVENT_CALLBACK))]
    static FMOD.RESULT BeatEventCallback(FMOD.Studio.EVENT_CALLBACK_TYPE type, FMOD.Studio.EventInstance instance, IntPtr parameterPtr)
    {
        IntPtr timelineInfoPtr;
        FMOD.RESULT result = instance.getUserData(out timelineInfoPtr);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogError("Timeline Callback error: " + result);
        }
        else if (timelineInfoPtr != IntPtr.Zero)
        {
            GCHandle timelineHandle = GCHandle.FromIntPtr(timelineInfoPtr);
            TimelineInfo timelineInfo = (TimelineInfo)timelineHandle.Target;

            switch (type)
            {
                case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT:
                    {
                        var parameter = (FMOD.Studio.TIMELINE_BEAT_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_BEAT_PROPERTIES));
                        timelineInfo.currentMusicBeat = parameter.beat;
                        beat = timelineInfo.currentMusicBeat;
                    }
                    break;
                case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER:
                    {
                        var parameter = (FMOD.Studio.TIMELINE_MARKER_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_MARKER_PROPERTIES));
                        timelineInfo.lastMarker = parameter.name;
                        marker = timelineInfo.lastMarker;
                    }
                    break;
            }
        }
        return FMOD.RESULT.OK;
    }

    /*
    int GetEventPos_FromEventEmitter(FMODUnity.StudioEventEmitter _eventEmitter)
    {
        FMOD.RESULT res;
        //int _eventPos;
        EventInstance[] _events;
        EventInstance eventInstance;
        //eventInstance.getTimelinePosition(out int position);
        //res = FMODUnity.RuntimeManager.GetEventDescription(_eventEmitter.Event).getInstanceList(out _events); // .Events;
        _eventEmitter.EventInstance.getTimelinePosition(out int position);
        //eventInstance = _events[0]; //return the first instance of the event
        //res = eventInstance.getTimelinePosition(out _eventPos);
        //Debug.Log(res);
        return position;//_eventPos;
    }
    */
}
