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

    /// <summary>  </summary>
    protected abstract void InitializeReplay();

    /// <summary> Tells the recorder script to start recording. </summary>
    public void StartRecording()
    {
        m_recorder.StartRecording();
    }

    /// <summary> Tells the recorder script to pause the recording. </summary>
    public void PauseRecording()
    {
        m_recorder.PauseRecording();
    }

    /// <summary> Tells the recorder script to continue the recording. </summary>
    public void ContinueRecording()
    {
        m_recorder.ContinueRecording();
    }

    /// <summary>
    /// Tells the recorder script to finish the recording.
    /// Creates a replay object and if the limit of replay objects are meet removes the first replay object in the list.
    /// </summary>
    public void StopRecording()
    {
        if(m_replayCount == m_numOfReplays)
            RemoveReplayObject();
        AddReplayObject();

        m_recorder.StopRecording();
    }

    public void ClearRecording()
    {
        m_recorder.ClearRecording();
    }

    /// <summary>  </summary>
    public void RestartReplays()
    {
        for(int i = 0; i < m_replayCount; i++)
            RestartReplay(i);
    }

    /// <summary>  </summary>
    public void RestartReplay(int _index)
    {
        m_replayObjects[_index].RestartReplay();
    }

    /// <summary>  </summary>
    public void PlayReplays()
    {
        for (int i = 0; i < m_replayCount; i++)
            PlayReplay(i);
    }

    /// <summary>  </summary>
    public void PlayReplay(int _index)
    {
        m_replayObjects[_index].PlayReplay();
    }

    /// <summary>  </summary>
    public void PauseReplays()
    {
        for (int i = 0; i < m_replayCount; i++)
            PauseReplay(i);
    }

    /// <summary>  </summary>
    public void PauseReplay(int _index)
    {
        m_replayObjects[_index].PauseReplay();
    }

    /// <summary>  </summary>
    public void StopReplays()
    {
        for (int i = 0; i < m_replayCount; i++)
            m_replayObjects[i].StopReplay();
    }

    /// <summary>  </summary>
    public void StopReplay(int _index)
    {
        m_replayObjects[_index].StopReplay();
    }

    /// <summary>  </summary>
    public void DeleteReplays()
    {
        for(int i = 0; i < m_replayCount; i++)
            Destroy(m_replayObjects[i].gameObject);

        m_replayObjects.Clear();
        m_replayCount = 0;
    }

    /// <summary>  </summary>
    public void DeleteReplay(int _index)
    {
        Destroy(m_replayObjects[_index].gameObject);
        m_replayObjects.RemoveAt(_index);
        m_replayCount--;
    }

    /// <summary>  </summary>
    public Transform GetReplayTransform(int _index)
    {
        return m_replayObjects[_index].gameObject.transform;
    }

    /// <summary>  </summary>
    public GameObject[] GetReplayObjects()
    {
        GameObject[] temp = new GameObject[m_replayCount];
        for(int i = 0; i < m_replayCount; i++)
        {
            temp[i] = m_replayObjects[i].gameObject;
        }
        return temp;
    }

    /// <summary>  </summary>
    public GameObject GetReplayObject(int _index)
    {
        return m_replayObjects[_index].gameObject;
    }

    /// <summary>  </summary>
    public bool HasReplays()
    {
        if(m_replayCount == 0)
            return false;
        return true;
    }

    /// <summary>  </summary>
    private void AddReplayObject()
    {
        m_replayTotalCount++;
        InitializeReplay();
        m_replayCount++;
    }

    /// <summary>  </summary>
    private void RemoveReplayObject()
    {
        Destroy(m_replayObjects[0].gameObject);
        m_replayObjects.RemoveAt(0);
        m_replayCount--;
    }
}
