using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReplaySystem_UIManager : MonoBehaviour
{
    [Header("Recording Management")]
    [SerializeField] private string m_recordingManagersTag;
    [SerializeField] private RecordingManager[] m_recordingManagers;

    [Space(20)]
    [Header("Buttons")]
    [SerializeField] private GameObject m_recordButton;
    [SerializeField] private GameObject m_finishRecordingButton;
    [SerializeField] private GameObject m_playReplayButton;
    [SerializeField] private GameObject m_pauseReplayButton;
    [SerializeField] private GameObject m_stopReplayButton;
    [SerializeField] private GameObject m_destroyAllButton;

    private Button m_playReplay;
    private Button m_stopReplay;
    private Button m_destroyAll;

    private void Awake()
    {
        if(m_recordingManagers.Length == 0)
        {
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(m_recordingManagersTag);
            m_recordingManagers = new RecordingManager[gameObjects.Length];
            for(int i = 0; i < gameObjects.Length; i++)
                m_recordingManagers[i] = gameObjects[i].GetComponent<RecordingManager>();
        }

        if (m_playReplayButton != null)
            m_playReplay = m_playReplayButton.GetComponentInChildren<Button>();
        if(m_stopReplayButton != null)
            m_stopReplay = m_stopReplayButton.GetComponentInChildren<Button>();
        if (m_destroyAllButton != null)
            m_destroyAll = m_destroyAllButton.GetComponentInChildren<Button>();

        DisableReplayButtons();
    }

    public void OnRecordClick()
    {
        foreach(RecordingManager manager in m_recordingManagers)
            manager.StopReplays();

        m_recordButton.SetActive(false);
        m_finishRecordingButton.SetActive(true);

        DisableReplayButtons();

        foreach (RecordingManager manager in m_recordingManagers)
            manager.StartRecording();
    }

    public void OnFinishRecordingClick()
    {
        m_recordButton.SetActive(true);
        m_finishRecordingButton.SetActive(false);

        ResetReplayButtons();

        foreach (RecordingManager manager in m_recordingManagers)
            manager.FinishRecording();
    }

    public void OnPlayReplayClick()
    {
        m_playReplayButton.SetActive(false);
        m_pauseReplayButton.SetActive(true);
        m_stopReplay.interactable = true;

        foreach (RecordingManager manager in m_recordingManagers)
            manager.PlayReplays();
    }

    public void OnPauseReplayClick()
    {
        m_pauseReplayButton.SetActive(false);
        m_playReplayButton.SetActive(true);

        foreach (RecordingManager manager in m_recordingManagers)
            manager.PauseReplays();
    }

    public void OnStopClick()
    {
        m_stopReplay.interactable = false;
        m_playReplayButton.SetActive(true);
        m_pauseReplayButton.SetActive(false);

        foreach (RecordingManager manager in m_recordingManagers)
            manager.StopReplays();
    }

    public void OnDestroyAllClick()
    {
        DisableReplayButtons();

        foreach (RecordingManager manager in m_recordingManagers)
            manager.DeleteReplays();
    }



    private void ResetReplayButtons()
    {
        m_playReplay.interactable = true;
        m_playReplayButton.SetActive(true);
        m_pauseReplayButton.SetActive(false);
        m_stopReplay.interactable = false;
        m_destroyAll.interactable = true;
    }

    private void DisableReplayButtons()
    {
        m_playReplay.interactable = false;
        m_playReplayButton.SetActive(true);
        m_pauseReplayButton.SetActive(false);
        m_stopReplay.interactable = false;
        m_destroyAll.interactable = false;
    }
}
