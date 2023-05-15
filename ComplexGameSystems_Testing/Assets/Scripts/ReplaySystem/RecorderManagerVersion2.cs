using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RecorderManagerVersion2 : MonoBehaviour
{
    [SerializeField] private Recorder m_recorder;
    [SerializeField, Range(1, 10)] private int m_numOfReplays = 1;
    [SerializeField] private ReplayObject m_replayObjectPrefab;

    // Replay Managers
    [SerializeField] private List<ReplayObject> m_replayObjects;
    private MemoryStreamSettings m_settings;

    private int m_replayCount = 0;

    private int m_replayTotalCount = 0;

    private void Awake()
    {
        m_replayObjects = new List<ReplayObject>(m_numOfReplays);
        if (m_recorder != null)
            m_settings = m_recorder.GetMemoryStreamSettings();
        else
            Debug.LogError(gameObject.name + " doesn't have a recorder set!");
    }

    public void StartRecording()
    {
        m_recorder.StartRecording();
    }

    public void FinishRecording()
    {
        if(m_replayCount == m_numOfReplays)
            RemoveReplayObject();
        AddReplayObject();

        m_recorder.StopRecording();
    }

    public void RestartReplay()
    {
        for(int i = 0; i < m_replayCount; i++)
        {
            m_replayObjects[i].RestartReplay();
        }
    }

    public void PlayReplay()
    {
        for (int i = 0; i < m_replayCount; i++)
        {
            m_replayObjects[i].PlayReplay();
        }
    }

    public void PauseReplay()
    {
        for (int i = 0; i < m_replayCount; i++)
        {
            m_replayObjects[i].PauseReplay();
        }
    }

    public void StopReplay()
    {
        for (int i = 0; i < m_replayCount; i++)
        {
            m_replayObjects[i].StopReplay();
        }
    }

    public void DeleteAllReplays()
    {
        for(int i = 0; i < m_replayCount; i++)
        {
            m_replayObjects[i].ClearMemory();
            Destroy(m_replayObjects[i].gameObject);
        }
        m_replayObjects.Clear();
        m_replayCount = 0;
    }


    private void AddReplayObject()
    {
        m_replayTotalCount++;
        ReplayObject replayObject = Instantiate(m_replayObjectPrefab);
        replayObject.InitializeReplayObject(m_recorder.GetMemoryStream(), m_settings, m_replayTotalCount);
        m_replayObjects.Add(replayObject);
        m_replayCount++;
    }

    private void RemoveReplayObject()
    {
        m_replayObjects[0].ClearMemory();
        Destroy(m_replayObjects[0].gameObject);
        m_replayObjects.RemoveAt(0);
        m_replayCount--;
    }
}
