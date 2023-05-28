using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarGameManager : Singleton<CarGameManager>
{
    [SerializeField] private ThirdPersonCar m_followCamera;
    [SerializeField] private PrometeoCarController m_carController;

    [SerializeField] private Canvas m_mainMenu;
    [SerializeField] private Canvas m_UICanvas;
    [SerializeField] private CarGameUI m_inGameUI;

    [SerializeField] private RecordingManager m_recordingManager;

    private Vector3 m_startPosition;
    private Vector3 m_startEularAngle;

    private void Start()
    {
        m_carController.isUserControlOn = false;
        m_startPosition = m_carController.gameObject.transform.position;
        m_startEularAngle = m_carController.gameObject.transform.eulerAngles;

        m_mainMenu.enabled = true;
        m_UICanvas.enabled = false;
    }

    public void OnStartPress()
    {
        m_recordingManager.StopReplay();

        m_mainMenu.enabled = false;
        m_UICanvas.enabled = true;

        m_carController.gameObject.transform.position = m_startPosition;
        m_carController.gameObject.transform.eulerAngles = m_startEularAngle;
        m_inGameUI.StartCountDown();
    }

    public void OnRestartPress()
    {
        m_recordingManager.StopReplay();

        m_carController.gameObject.transform.position = m_startPosition;
        m_carController.gameObject.transform.eulerAngles = m_startEularAngle;
        m_inGameUI.StartCountDown();
    }

    public void OnWatchReplayPress()
    {
        m_recordingManager.StopReplay();
        m_carController.gameObject.SetActive(false);

        int replayCount = m_recordingManager.m_replayCount;
        m_recordingManager.RestartReplay(replayCount - 1);
        m_followCamera.SetFollowTarget(m_recordingManager.GetReplayTransform(replayCount - 1));
        
    }

    public void OnQuitPress()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }

    public void OnCheckPointTrigger(bool _finishPoint)
    {
        m_inGameUI.CheckPointHit(_finishPoint);
    }

    public void FinishedLinePassed(float _finishTime)
    {
        m_recordingManager.FinishRecording();
        m_carController.isUserControlOn = false;
        m_carController.Brakes();
        Debug.Log("Finished :" + _finishTime.ToString());



        // TODO: Put code for finishing
        // Menu swaping
        //m_followCamera.SetFollowTarget(null);
    }

    public void StartRace()
    {
        m_carController.isUserControlOn = true;
        m_recordingManager.StartRecording();
        if(m_recordingManager.HasReplayObjects())
        {
            m_recordingManager.RestartReplay();
        }
    }

    private void ResetRace()
    {

    }
}