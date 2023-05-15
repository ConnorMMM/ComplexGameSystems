using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ReplayObject : MonoBehaviour
{
    private MemoryStream m_memoryStream = null;
    private BinaryReader m_binaryReader = null;

    private MemoryStreamSettings m_settings;

    private bool replaying = false;

    private void OnDestroy()
    {
        ClearMemory();
    }

    private void FixedUpdate()
    {
        if (replaying)
            UpdateReplay();
    }

    private void UpdateReplay()
    {
        if (m_memoryStream.Position >= m_memoryStream.Length)
        {
            PauseReplay();
            return;
        }

        if (m_settings.UsePosition()) ApplyReplayPosition();
        if (m_settings.UseRotation()) ApplyReplayRotation();
        if (m_settings.UseScale()) ApplyReplayScale();
    }

    public void InitializeReplayObject(MemoryStream _memoryStream, MemoryStreamSettings _settings, int _number)
    {
        gameObject.name = "ReplayObject_" +  _number.ToString();
        m_settings = _settings;
        m_memoryStream = _memoryStream;
        m_binaryReader = new BinaryReader(_memoryStream);
        ResetReplay();
        SetVisibility(false);
        replaying = false;
    }

    public bool RestartReplay()
    {
        if (m_memoryStream == null)
        {
            Debug.Log($"MemoryStream is not set for {gameObject}");
            return false;
        }

        ResetReplay();
        SetVisibility(true);
        replaying = true;

        return true;
    }

    public void PlayReplay()
    {
        if(m_memoryStream == null)
        {
            Debug.Log($"MemoryStream is not set for {gameObject}");
            return;
        }

        SetVisibility(true);
        replaying = true;
    }

    public void PauseReplay()
    {
        replaying = false;
    }

    public void StopReplay()
    {
        SetVisibility(false);
        replaying = false;
    }

    public void ClearMemory()
    {
        m_memoryStream.Dispose();
        m_binaryReader.Dispose();
    }

    private void SetVisibility(bool _state)
    {
        gameObject.SetActive(_state);
    }

    private void ResetReplay()
    {
        m_memoryStream.Seek(0, SeekOrigin.Begin);
        ApplyInitialFrame();
    }


    protected void ApplyInitialFrame()
    {
        if (m_settings.UsePosition())
        {
            float x = m_binaryReader.ReadSingle();
            float y = m_binaryReader.ReadSingle();
            float z = m_binaryReader.ReadSingle();
            transform.position = new Vector3(x, y, z);
        }
        if (m_settings.UseRotation())
        {
            float x = m_binaryReader.ReadSingle();
            float y = m_binaryReader.ReadSingle();
            float z = m_binaryReader.ReadSingle();
            transform.rotation = new Quaternion(x, y, z, 0);
        }
        if (m_settings.UseScale())
        {
            float x = m_binaryReader.ReadSingle();
            float y = m_binaryReader.ReadSingle();
            float z = m_binaryReader.ReadSingle();
            transform.localScale = new Vector3(x, y, z);
        }
    }

    protected void ApplyReplayPosition()
    {
        Debug.Log("Applying Position");
        if (m_binaryReader.ReadBoolean())
        {
            Vector3 recPos;
            recPos.x = m_binaryReader.ReadSingle();
            recPos.y = m_binaryReader.ReadSingle();
            recPos.z = m_binaryReader.ReadSingle();
            transform.position += recPos;
        }
    }

    protected void ApplyReplayRotation()
    {
        Debug.Log("Applying Rotation");
        if (m_binaryReader.ReadBoolean())
        {
            Vector3 recRot;
            recRot.x = m_binaryReader.ReadSingle();
            recRot.y = m_binaryReader.ReadSingle();
            recRot.z = m_binaryReader.ReadSingle();
            transform.rotation = new Quaternion(transform.rotation.x + recRot.x, transform.rotation.y + recRot.y, transform.rotation.z + recRot.z, 0);
        }
    }

    protected void ApplyReplayScale()
    {
        Debug.Log("Applying Scale");
        if (m_binaryReader.ReadBoolean())
        {
            Vector3 recScale;
            recScale.x = m_binaryReader.ReadSingle();
            recScale.y = m_binaryReader.ReadSingle();
            recScale.z = m_binaryReader.ReadSingle();
            transform.localScale += recScale;
        }
    }
}
