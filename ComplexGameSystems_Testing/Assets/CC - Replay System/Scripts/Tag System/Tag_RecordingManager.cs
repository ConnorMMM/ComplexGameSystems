using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tag_RecordingManager : RecordingManager
{
    [SerializeField] private string[] m_recordingTags;

    // Replay Managers
    private GameObject[] m_recorderObjects;

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
        Tag_Recorder temp = gameObject.AddComponent<Tag_Recorder>();
        temp.SetGameObjects(m_recorderObjects);
        m_recorder = temp;

        base.Awake();
    }

    protected override void InitializeReplay()
    {
        ReplayObject replayObject = new GameObject().AddComponent<Tag_ReplayObject>();
        replayObject.InitializeReplay(m_recorderObjects, m_recorder.GetMemoryStream(), $"{gameObject.name}_ReplayObjectList_{m_replayTotalCount}");
        m_replayObjects.Add(replayObject);
    }
}
