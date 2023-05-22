using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tag_Recorder : Recorder
{
    private GameObject[] m_gameObjects = new GameObject[0];
    private new MemoryStreamSettings m_settings;

    private Vector3[] m_PositionsPrevFrame;
    private Vector3[] m_RotationsPrevFrame;
    private Vector3[] m_ScalesPrevFrame;

    protected override void CollectPreviousFrame()
    {
        if(m_gameObjects.Length == 0)
            return;

        if (m_settings.UsePosition())
        {
            for(int i = 0; i < m_gameObjects.Length; i++)
                m_PositionsPrevFrame[i] = m_gameObjects[i].transform.position;
        }
        if (m_settings.UseRotation())
        {
            for (int i = 0; i < m_gameObjects.Length; i++)
                m_RotationsPrevFrame[i] = m_gameObjects[i].transform.localEulerAngles;
        }
        if (m_settings.UseScale())
        {
            for (int i = 0; i < m_gameObjects.Length; i++)
                m_ScalesPrevFrame[i] = m_gameObjects[i].transform.lossyScale;
        }
    }

    protected override void RecordInitialFrame()
    {
        if (m_gameObjects.Length == 0)
            return;

        if (m_settings.UsePosition())
        {
            for (int i = 0; i < m_gameObjects.Length; i++)
                SaveVector3(m_gameObjects[i].transform.position);
        }
        if (m_settings.UseRotation())
        {
            for (int i = 0; i < m_gameObjects.Length; i++)
                SaveVector3(m_gameObjects[i].transform.localEulerAngles);
        }
        if (m_settings.UseScale())
        {
            for (int i = 0; i < m_gameObjects.Length; i++)
                SaveVector3(m_gameObjects[i].transform.lossyScale);
        }
    }

    protected override void UpdateRecording()
    {
        if (m_gameObjects.Length == 0)
            return;

        if (m_settings.UsePosition())
        {
            for (int i = 0; i < m_gameObjects.Length; i++)
                CheckVector3Diff(m_gameObjects[i].transform.position, m_PositionsPrevFrame[i]);
        }
        if (m_settings.UseRotation())
        {
            for (int i = 0; i < m_gameObjects.Length; i++)
                CheckVector3Diff(m_gameObjects[i].transform.localEulerAngles, m_RotationsPrevFrame[i]);
        }
        if (m_settings.UseScale())
        {
            for (int i = 0; i < m_gameObjects.Length; i++)
                CheckVector3Diff(m_gameObjects[i].transform.lossyScale, m_ScalesPrevFrame[i]);
        }
    }

    public void SetGameObjects(GameObject[] _gameObjects)
    {
        m_gameObjects = _gameObjects;
        m_PositionsPrevFrame = new Vector3[m_gameObjects.Length];
        m_RotationsPrevFrame = new Vector3[m_gameObjects.Length];
        m_ScalesPrevFrame = new Vector3[m_gameObjects.Length];
    }

    public void SetSettings(MemoryStreamSettings _settings)
    {
        this.m_settings = _settings;
    }
}
