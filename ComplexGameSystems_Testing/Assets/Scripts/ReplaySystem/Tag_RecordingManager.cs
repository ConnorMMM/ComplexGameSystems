using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tag_RecordingManager : RecordingManager
{
    [SerializeField] private new MemoryStreamSettings m_settings;
    [SerializeField] private string[] m_recordingTags;

    // Replay Managers
    public GameObject[] m_recorderObjects;

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
        temp.SetSettings(m_settings);
        m_recorder = temp;

        base.Awake();
    }

    protected override void InitializeReplay()
    {
        ReplayObject replayObject = Instantiate(m_replayObjectPrefab);
        replayObject.InitializeReplay(m_recorderObjects, m_recorder.GetMemoryStream(), m_settings, $"{gameObject.name}_ReplayObjectList_{m_replayTotalCount}");
        m_replayObjects.Add(replayObject);
    }
}
