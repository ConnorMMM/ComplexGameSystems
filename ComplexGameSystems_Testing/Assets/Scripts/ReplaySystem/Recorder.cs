using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class Recorder : MonoBehaviour
{
    [SerializeField] private MemoryStreamSettings m_settings;

    private Vector3 m_PositionPrevFrame;
    private Vector3 m_RotationPrevFrame;
    private Vector3 m_ScalePrevFrame;

    private MemoryStream memoryStream = null;
    private BinaryWriter binaryWriter = null;

    private bool recordingInitialized;
    private bool recording;

    private void FixedUpdate()
    {
        if (recording)
        {
            UpdateRecording();
        }

        if(m_settings.UsePosition()) m_PositionPrevFrame = transform.position;
        if(m_settings.UseRotation()) m_RotationPrevFrame = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);
        if (m_settings.UseScale()) m_ScalePrevFrame = transform.lossyScale;
    }

    private void UpdateRecording()
    {
        if (m_settings.UsePosition()) SavePosition();
        if (m_settings.UseRotation()) SaveRotation();
        if (m_settings.UseScale()) SaveScale();
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
        RecordInitialFrame();
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

    private void SavePosition()
    {
        Debug.Log("Saving Position");
        if (m_PositionPrevFrame == transform.position)
        {
            binaryWriter.Write(false);
        }
        else
        {
            binaryWriter.Write(true);
            binaryWriter.Write(transform.position.x - m_PositionPrevFrame.x);
            binaryWriter.Write(transform.position.y - m_PositionPrevFrame.y);
            binaryWriter.Write(transform.position.z - m_PositionPrevFrame.z);
        }
    }
    private void SaveRotation()
    {
        Debug.Log("Saving Rotation");
        Vector3 curRot = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);
        if (m_RotationPrevFrame == curRot)
        {
            binaryWriter.Write(false);
        }
        else
        {
            binaryWriter.Write(true);
            binaryWriter.Write(curRot.x - m_RotationPrevFrame.x);
            binaryWriter.Write(curRot.y - m_RotationPrevFrame.y);
            binaryWriter.Write(curRot.z - m_RotationPrevFrame.z);
        }
    }
    private void SaveScale()
    {
        Debug.Log("Saving Scale");
        if (m_ScalePrevFrame == transform.lossyScale)
        {
            binaryWriter.Write(false);
        }
        else
        {
            binaryWriter.Write(true);
            binaryWriter.Write(transform.lossyScale.x - m_ScalePrevFrame.x);
            binaryWriter.Write(transform.lossyScale.y - m_ScalePrevFrame.y);
            binaryWriter.Write(transform.lossyScale.z - m_ScalePrevFrame.z);
        }
    }

    public MemoryStream GetMemoryStream()
    {
        MemoryStream tempStream = memoryStream;
        StopRecording();
        InitializeRecording();
        return tempStream;
    }

    public MemoryStreamSettings GetMemoryStreamSettings() { return m_settings; }

    protected void RecordInitialFrame()
    {
        if (m_settings.UsePosition())
        {
            binaryWriter.Write(transform.position.x);
            binaryWriter.Write(transform.position.y);
            binaryWriter.Write(transform.position.z);
        }
        if (m_settings.UseRotation())
        {
            binaryWriter.Write(transform.rotation.x);
            binaryWriter.Write(transform.rotation.y);
            binaryWriter.Write(transform.rotation.z);
        }
        if (m_settings.UseScale())
        {
            binaryWriter.Write(transform.lossyScale.x);
            binaryWriter.Write(transform.lossyScale.y);
            binaryWriter.Write(transform.lossyScale.z);
        }
    }
}
