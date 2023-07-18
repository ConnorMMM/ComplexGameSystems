using System;
using System.IO;
using UnityEngine;

public abstract class Recorder : MonoBehaviour
{
    private MemoryStream m_memoryStream = null;
    private BinaryWriter m_binaryWriter = null;

    private bool recordingInitialized;
    private bool recording;
    private bool isPaused;

    private void OnDestroy()
    {
        ClearMemory();
    }

    private void FixedUpdate()
    {
        if (recording)
        {
            if(!isPaused)
                UpdateRecording();
        }

        if (!isPaused)
            CollectPreviousFrame();
    }

    /// <summary>
    /// Adds the change between the current frame and the previously recorded frame to the MemoryStream.
    /// </summary>
    protected abstract void UpdateRecording();

    /// <summary>
    /// Adds the starting values of the object to the MemoryStream.
    /// </summary>
    protected abstract void RecordInitialFrame();

    /// <summary>
    /// Stores the previous frame's values for later use.
    /// </summary>
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
        isPaused = false;
    }

    public void PauseRecording()
    {
        if (!recordingInitialized)
            throw new InvalidOperationException("MemoryStream is not initialized");

        isPaused = true;
    }

    public void ContinueRecording()
    {
        if (!recordingInitialized)
            throw new InvalidOperationException("MemoryStream is not initialized");

        isPaused = false;
    }

    public void StopRecording()
    {
        recording = false;
    }

    public void ClearRecording()
    {
        StopRecording();
        ClearMemory();
    }

    public MemoryStream GetMemoryStream()
    {
        MemoryStream tempStream = m_memoryStream;
        StopRecording();
        InitializeRecording();
        return tempStream;
    }


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

    /// <summary>
    /// Saves a Bool to the MemoryStream.
    /// </summary>
    protected void SaveBool(bool _data)
    {
        m_binaryWriter.Write(_data);
    }

    /// <summary>
    /// Saves a Vector2 to the MemoryStream.
    /// </summary>
    protected void SaveVector2(Vector2 _data)
    {
        m_binaryWriter.Write(_data.x);
        m_binaryWriter.Write(_data.y);
    }

    /// <summary>
    /// Saves a Vector3 to the MemoryStream.
    /// </summary>
    protected void SaveVector3(Vector3 _data)
    {
        m_binaryWriter.Write(_data.x);
        m_binaryWriter.Write(_data.y);
        m_binaryWriter.Write(_data.z);
    }

    /// <summary>
    /// If the two Vector2 are different saves a true bool value and the difference between the two, 
    /// else it just saves a false bool value.
    /// </summary>
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

    /// <summary>
    /// If the two Vector3 are different saves a true bool value and the difference between the two, 
    /// else it just saves a false bool value.
    /// </summary>
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

    private void ClearMemory()
    {
        if (m_memoryStream != null)
        {
            m_memoryStream.Dispose();
            m_binaryWriter.Dispose();
        }
        recordingInitialized = false;
    }
}
