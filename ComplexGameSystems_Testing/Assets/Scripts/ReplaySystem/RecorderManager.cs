using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RecorderManager : MonoBehaviour
{
    [SerializeField] private Recorder m_recorder;
    [SerializeField, Range(1, 10)] private int m_numOfReplays = 1;
    [SerializeField] private GameObject m_replayObjectPrefab;

    // Replay Managers
    [SerializeField] private List<GameObject> m_replayObjects;
    [SerializeField] private List<MemoryStream> m_memoryStreams;
    [SerializeField] private List<BinaryReader> m_binaryReader;
    [SerializeField] private List<bool> m_replaying;

    private int m_replayCount = 0;
    private bool m_replayObjectsInitialized = false;

    private void Awake()
    {
        m_replayObjects = new List<GameObject>(m_numOfReplays);
        m_memoryStreams = new List<MemoryStream>(m_numOfReplays);
        m_binaryReader = new List<BinaryReader>(m_numOfReplays);
    }

    private void OnDestroy()
    {
        DestroyReplays();
        RemoveAllMemoryStreams();
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < m_replayCount; i++)
        {
            if (m_replaying[i])
            {
                UpdateReplay(i);
            }
        }
    }

    private void UpdateReplay(int _replayIndex)
    {
        if (m_memoryStreams[_replayIndex].Position >= m_memoryStreams[_replayIndex].Length)
        {
            m_replaying[_replayIndex] = false;
            return;
        }
        ApplyReplayFrame(_replayIndex);
    }

    public void StartRecording()
    {
        m_recorder.StartRecording();
    }

    public void FinishRecording()
    {
        if (m_numOfReplays == 1)
        {
            m_memoryStreams.Insert(0, m_recorder.GetMemoryStream());
            m_binaryReader.Insert(0, new BinaryReader(m_memoryStreams[0]));
            m_memoryStreams[0].Seek(0, SeekOrigin.Begin);
            m_replaying[0] = false;
            m_replayCount = 1;
        }
        else if (m_replayCount < m_numOfReplays)
        {
            m_memoryStreams.Insert(m_replayCount, m_recorder.GetMemoryStream());
            m_binaryReader.Insert(m_replayCount, new BinaryReader(m_memoryStreams[m_replayCount]));
            m_memoryStreams[m_replayCount].Seek(0, SeekOrigin.Begin);
            m_replaying[m_replayCount] = false;
            m_replayCount++;
        }
        else
        {
            m_memoryStreams[0].Dispose();
            m_binaryReader[0].Dispose();
            Destroy(m_replayObjects[0]);
            for (int i = m_replayCount - 2; i >= 0; i--)
            {
                m_memoryStreams.Insert(i, m_memoryStreams[i + 1]);
                m_binaryReader.Insert(i, m_binaryReader[i + 1]);
                m_replayObjects.Insert(i, m_replayObjects[i + 1]);
                m_replaying.Insert(i, m_replaying[i + 1]);
            }
            m_memoryStreams.Insert(m_replayCount - 1, m_recorder.GetMemoryStream());
            m_binaryReader.Insert(m_replayCount - 1, new BinaryReader(m_memoryStreams[m_replayCount - 1]));
            m_memoryStreams[m_replayCount - 1].Seek(0, SeekOrigin.Begin);
            m_replaying[m_replayCount] = false;
        }

        m_recorder.StopRecording();
    }

    public void RestartReplay()
    {
        if(!m_replayObjectsInitialized)
            InializeReplays();

        for(int i = 0; i < m_replayCount; i++)
        {
            m_memoryStreams[i].Seek(0, SeekOrigin.Begin);
            ApplyInitialFrame(i);
        }

        //replaying = true;
    }

    public void PlayReplay()
    {
        if (!m_replayObjectsInitialized)
            InializeReplays();

        //replaying = true;
    }

    public void PauseReplay()
    {
        if (!m_replayObjectsInitialized)
            return;

        //replaying = false;
    }

    private void InializeReplays()
    {
        if (m_replayObjectsInitialized)
            DestroyReplays();

        for (int i = 0; i < m_replayCount; i++)
        {
            m_replayObjects.Insert(i, Instantiate(m_replayObjectPrefab));
            m_memoryStreams[i].Seek(0, SeekOrigin.Begin);
            ApplyInitialFrame(i);
        }
        m_replayObjectsInitialized = true;
    }

    private void DestroyReplays()
    {
        if (!m_replayObjectsInitialized)
            return;

        for(int i = 0; i < m_replayCount; i++)
        {
            Destroy(m_replayObjects[i]);
        }

        m_replayObjectsInitialized = false;
    }

    private void RemoveAllMemoryStreams()
    {
        for (int i = 0; i < m_replayCount; i++)
        {
            m_memoryStreams[i].Dispose();
            m_binaryReader[i].Dispose();
        }
        m_replayCount = 0;
    }

    protected void ApplyInitialFrame(int _replayIndex)
    {
        float x = m_binaryReader[_replayIndex].ReadSingle();
        float y = m_binaryReader[_replayIndex].ReadSingle();
        float z = m_binaryReader[_replayIndex].ReadSingle();
        Debug.Log($"x: {x}, y: {y}, z: {z}");
        m_replayObjects[_replayIndex].transform.position = new Vector3(x, y, z);
    }

    protected void ApplyReplayFrame(int _replayIndex)
    {
        if (m_binaryReader[_replayIndex].ReadBoolean())
        {
            float x = m_binaryReader[_replayIndex].ReadSingle();
            float y = m_binaryReader[_replayIndex].ReadSingle();
            float z = m_binaryReader[_replayIndex].ReadSingle();
            Debug.Log($"x: {x}, y: {y}, z: {z}");
            Vector3 curPos = m_replayObjects[_replayIndex].transform.position;
            m_replayObjects[_replayIndex].transform.position = new Vector3(curPos.x + x, curPos.y + y, curPos.z + z);
        }
        else
        {
            Debug.Log("Empty");
        }
    }
}
