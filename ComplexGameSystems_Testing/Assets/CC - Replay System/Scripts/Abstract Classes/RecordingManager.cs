using System.Collections.Generic;
using UnityEngine;

public abstract class RecordingManager : MonoBehaviour
{
    [SerializeField] protected Recorder m_recorder;
    [SerializeField] protected ReplayObject m_replayObjectPrefab;
    [SerializeField, Range(1, 10)] private int m_numOfReplays = 1;

    // Replay Managers
    [SerializeField] protected List<ReplayObject> m_replayObjects;

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

    public void RestartReplays()
    {
        for(int i = 0; i < m_replayCount; i++)
            RestartReplay(i);
    }

    public void RestartReplay(int _index)
    {
        m_replayObjects[_index].RestartReplay();
    }

    public void PlayReplays()
    {
        for (int i = 0; i < m_replayCount; i++)
            PlayReplay(i);
    }

    public void PlayReplay(int _index)
    {
        m_replayObjects[_index].PlayReplay();
    }

    public void PauseReplays()
    {
        for (int i = 0; i < m_replayCount; i++)
            PauseReplay(i);
    }

    public void PauseReplay(int _index)
    {
        m_replayObjects[_index].PauseReplay();
    }

    public void StopReplays()
    {
        for (int i = 0; i < m_replayCount; i++)
            m_replayObjects[i].StopReplay();
    }

    public void StopReplay(int _index)
    {
        m_replayObjects[_index].StopReplay();
    }

    public void DeleteReplays()
    {
        for(int i = 0; i < m_replayCount; i++)
            Destroy(m_replayObjects[i].gameObject);

        m_replayObjects.Clear();
        m_replayCount = 0;
    }

    public void DeleteReplay(int _index)
    {
        Destroy(m_replayObjects[_index].gameObject);
        m_replayObjects.RemoveAt(_index);
        m_replayCount--;
    }

    public Transform GetReplayTransform(int _index)
    {
        return m_replayObjects[_index].gameObject.transform;
    }

    public GameObject[] GetReplayObjects()
    {
        GameObject[] temp = new GameObject[m_replayCount];
        for(int i = 0; i < m_replayCount; i++)
        {
            temp[i] = m_replayObjects[i].gameObject;
        }
        return temp;
    }

    public GameObject GetReplayObject(int _index)
    {
        return m_replayObjects[_index].gameObject;
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
