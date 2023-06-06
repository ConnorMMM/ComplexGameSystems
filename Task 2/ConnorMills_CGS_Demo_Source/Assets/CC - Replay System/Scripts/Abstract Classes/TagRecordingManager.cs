using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public abstract class TagRecordingManager : RecordingManager
{
    [SerializeField] private string[] m_recordingTags;

    // Replay Managers
    protected GameObject[] m_recorderObjects;

    protected override void Awake()
    {
        m_recorderObjects = new GameObject[0];
        foreach (string tag in m_recordingTags)
        {
            m_recorderObjects = m_recorderObjects.Concat(GameObject.FindGameObjectsWithTag(tag)).ToArray();
        }
        if(m_recorderObjects.Length == 0)
        {
            enabled = false;
            return;
        }
        
        TagRecorder recorder = CreateRecorder();
        if (recorder == null)
            throw new ArgumentException("Incorrect Recorder assigned to manager");
        recorder.SetGameObjects(m_recorderObjects);
        m_recorder = recorder;

        base.Awake();
    }

    protected override void InitializeReplay()
    {
        ReplayObject replayObject = CreateReplayObject();
        if (replayObject == null)
            throw new ArgumentException("Incorrect Replay Object assigned to manager");
        replayObject.InitializeReplay(m_recorderObjects, m_recorder.GetMemoryStream(), $"{gameObject.name}_ReplayObjectList_{m_replayTotalCount}");
        m_replayObjects.Add(replayObject);
    }

    protected abstract TagRecorder CreateRecorder();

    protected abstract TagReplayObject CreateReplayObject();
}
