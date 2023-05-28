using System.Collections.Generic;
using UnityEngine;

public abstract class RecordingManager : MonoBehaviour
{
    [SerializeField] protected Recorder m_recorder;
    [SerializeField] protected ReplayObject m_replayObjectPrefab;
    [SerializeField, Range(1, 10)] private int m_numOfReplays = 1;

    // Replay Managers
    [SerializeField] protected List<ReplayObject> m_replayObjects;
    protected MemoryStreamSettings m_settings;

    public int m_replayCount { get; private set; }
    protected int m_replayTotalCount = 0;

    protected virtual void Awake()
    {
        m_replayObjects = new List<ReplayObject>(m_numOfReplays);
        m_replayCount = 0;
    }

    protected abstract void InitializeReplay();

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

    public void PlayReplay(int _index)
    {
        m_replayObjects[_index].PlayReplay();
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
            Destroy(m_replayObjects[i].gameObject);

        m_replayObjects.Clear();
        m_replayCount = 0;
    }

    public Transform GetReplayTransform(int _index)
    {
        return m_replayObjects[_index].gameObject.transform;
    }

    public bool HasReplayObjects()
    {
        if(m_replayCount == 0)
            return false;
        return true;
    }


    private void AddReplayObject()
    {
        m_replayTotalCount++;
        InitializeReplay();
        m_replayCount++;
    }

    private void RemoveReplayObject()
    {
        Destroy(m_replayObjects[0].gameObject);
        m_replayObjects.RemoveAt(0);
        m_replayCount--;
    }
}
