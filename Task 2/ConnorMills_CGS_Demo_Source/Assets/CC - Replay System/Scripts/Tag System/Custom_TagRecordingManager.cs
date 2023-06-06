using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Custom_TagRecordingManager : TagRecordingManager
{
    protected override TagRecorder CreateRecorder()
    {
        return Instantiate(m_recorder, transform) as TagRecorder;
    }

    protected override TagReplayObject CreateReplayObject()
    {
        return Instantiate(m_replayObjectPrefab) as TagReplayObject;
    }
}
