using System.IO;
using UnityEngine;

public abstract class Recorder : MonoBehaviour
{
    private MemoryStream m_memoryStream = null;
    protected BinaryWriter m_binaryWriter = null;
    [SerializeField] protected MemoryStreamSettings m_settings;

    private bool recordingInitialized;
    private bool recording;

    private void OnDestroy()
    {
        m_memoryStream.Dispose();
        m_binaryWriter.Dispose();
    }

    private void FixedUpdate()
    {
        if (recording)
            UpdateRecording();

        CollectPreviousFrame();
    }

    protected abstract void UpdateRecording();

    protected abstract void RecordInitialFrame();

    protected abstract void CollectPreviousFrame();


    public void StartRecording()
    {
        if(!recordingInitialized)
            InitializeRecording();
        else
            m_memoryStream.SetLength(0);

        ResetReplayFrame();
        RecordInitialFrame();
        recording = true;
    }

    public void StopRecording()
    {
        recording = false;
    }

    public MemoryStream GetMemoryStream()
    {
        MemoryStream tempStream = m_memoryStream;
        StopRecording();
        InitializeRecording();
        return tempStream;
    }

    public MemoryStreamSettings GetMemoryStreamSettings() { return m_settings; }


    private void InitializeRecording()
    {
        m_memoryStream = new MemoryStream();
        m_binaryWriter = new BinaryWriter(m_memoryStream);
        recordingInitialized = true;
    }

    private void ResetReplayFrame()
    {
        m_memoryStream.Seek(0, SeekOrigin.Begin);
        m_binaryWriter.Seek(0, SeekOrigin.Begin);
    }


    protected void SaveBool(bool _data)
    {
        m_binaryWriter.Write(_data);
    }

    protected void SaveVector2(Vector2 _data)
    {
        m_binaryWriter.Write(_data.x);
        m_binaryWriter.Write(_data.y);
    }

    protected void SaveVector3(Vector3 _data)
    {
        m_binaryWriter.Write(_data.x);
        m_binaryWriter.Write(_data.y);
        m_binaryWriter.Write(_data.z);
    }

    protected void CheckVector2Diff(Vector2 _cur, Vector2 _prev)
    {
        if (_prev == _cur)
            m_binaryWriter.Write(false);
        else
        {
            m_binaryWriter.Write(true);
            SaveVector2(_cur - _prev);
        }
    }

    protected void CheckVector3Diff(Vector3 _cur, Vector3 _prev)
    {
        if (_prev == _cur)
            m_binaryWriter.Write(false);
        else
        {
            m_binaryWriter.Write(true);
            SaveVector3(_cur - _prev);
        }
    }
}
