using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Recorder : MonoBehaviour
{
    public MemoryStream recordingStream { get; private set; }

    private Transform m_transform;
    private Vector3 m_PositionPrevFrame;
    private Vector3 m_RotationPrevFrame;
    private Vector3 m_ScalePrevFrame;

    private MemoryStream memoryStream = null;
    private BinaryWriter binaryWriter = null;

    private bool recordingInitialized;
    private bool recording;

    private void Awake()
    {
        m_transform = gameObject.transform;
    }

    private void FixedUpdate()
    {
        if (recording)
        {
            UpdateRecording();
        }

        m_PositionPrevFrame = m_transform.position;
        m_RotationPrevFrame = new Vector3(m_transform.rotation.x, m_transform.rotation.y, m_transform.rotation.z);
        m_ScalePrevFrame = m_transform.lossyScale;
    }

    private void UpdateRecording()
    {
        SaveTransform();
    }

    private void InitializeRecording()
    {
        memoryStream = new MemoryStream();
        binaryWriter = new BinaryWriter(memoryStream);
        recordingInitialized = true;
    }

    public void StartRecording()
    {
        if(!recordingInitialized)
            InitializeRecording();
        else
            memoryStream.SetLength(0);

        ResetReplayFrame();
        recording = true;
    }

    public void StopRecording()
    {
        recording = false;
    }

    private void ResetReplayFrame()
    {
        memoryStream.Seek(0, SeekOrigin.Begin);
        binaryWriter.Seek(0, SeekOrigin.Begin);
    }

    private void SaveTransform()
    {
        if(m_PositionPrevFrame == transform.position)
        {
            binaryWriter.Write(false);
            Debug.Log("Recording: Empty");
        }
        else
        {
            binaryWriter.Write(true);
            binaryWriter.Write(m_PositionPrevFrame.x - m_transform.position.x);
            binaryWriter.Write(m_PositionPrevFrame.y - m_transform.position.y);
            binaryWriter.Write(m_PositionPrevFrame.z - m_transform.position.z);
            Debug.Log($"Recording: ({m_PositionPrevFrame.x - m_transform.position.x}, {m_PositionPrevFrame.y - m_transform.position.y}, {m_PositionPrevFrame.z - m_transform.position.z})");
        }
    }

    public MemoryStream GetMemoryStream()
    {
        MemoryStream tempStream = memoryStream;
        StopRecording();
        InitializeRecording();
        return tempStream;
    }
}
