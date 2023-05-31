using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transform_TagRecordingManager : TagRecordingManager
{
    protected override TagRecorder CreateRecorder()
    {
        return Instantiate(new GameObject(), transform).AddComponent<Transform_TagRecorder>();
    }

    protected override TagReplayObject CreateReplayObject()
    {
        return new GameObject().AddComponent<Transform_TagReplayObject>();
    }
}
