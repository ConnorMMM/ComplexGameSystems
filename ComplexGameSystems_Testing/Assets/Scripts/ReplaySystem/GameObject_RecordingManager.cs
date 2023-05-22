using System.Collections.Generic;
using UnityEngine;

public class GameObject_RecordingManager : RecordingManager
{
    private string m_gameObjectName = "";

    protected override void Awake()
    {
        if (m_recorder != null)
        {
            m_settings = m_recorder.GetMemoryStreamSettings();
            m_gameObjectName = m_recorder.gameObject.name;
        }
        else
            Debug.LogError(gameObject.name + " doesn't have a recorder set!");

        base.Awake();
    }

    protected override void InitializeReplay()
    {
        ReplayObject replayObject = Instantiate(m_replayObjectPrefab);
        replayObject.InitializeReplay(new GameObject[0], m_recorder.GetMemoryStream(), m_settings, $"{m_gameObjectName}_ReplayObject_{m_replayTotalCount}");
        m_replayObjects.Add(replayObject);
    }
}