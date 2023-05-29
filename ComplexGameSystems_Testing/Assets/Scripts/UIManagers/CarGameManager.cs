using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarGameManager : Singleton<CarGameManager>
{
    [SerializeField] private ThirdPersonCar m_followCamera;
    [SerializeField] private PrometeoCarController m_carController;
    [SerializeField] private Transform m_finishCameraSpot;

    [SerializeField] private Canvas m_mainMenuCanvas;
    [SerializeField] private Canvas m_UICanvas;
    [SerializeField] private Canvas m_replayCustomiserCanvas;
    [SerializeField] private Canvas m_leaderboardCanvas;


    [SerializeField] private RecordingManager m_recordingManager;

    private CarGameUI m_UIManager;

    private Vector3 m_startPosition;
    private Vector3 m_startEularAngle;

    private void Start()
    {
        m_carController.isUserControlOn = false;
        m_startPosition = m_carController.gameObject.transform.position;
        m_startEularAngle = m_carController.gameObject.transform.eulerAngles;

        GoToMainMenu();

        m_UIManager = m_UICanvas.GetComponent<CarGameUI>();
    }

    public void OnStartPress()
    {
        m_recordingManager.RestartReplays();
        m_recordingManager.PauseReplays();

        GoToInGameUI();
        InitialiseRace();
    }

    public void OnRestartPress()
    {
        m_recordingManager.RestartReplays();
        m_recordingManager.PauseReplays();

        GoToInGameUI();
        InitialiseRace();
    }

    public void OnWatchReplayPress()
    {
        m_recordingManager.StopReplays();
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

    public void GoToMainMenu()
    {
        SetCanvasesState(false);
        m_mainMenuCanvas.enabled = true;
    }

    public void GoToInGameUI()
    {
        SetCanvasesState(false);
        m_UICanvas.enabled = true;
    }

    public void GoToReplayCustomiser()
    {
        SetCanvasesState(false);
        m_replayCustomiserCanvas.enabled = true;
    }

    public void GoToLeaderboard()
    {
        SetCanvasesState(false);
        m_leaderboardCanvas.enabled = true;
    }

    public void InitialiseRace()
    {
        m_carController.gameObject.SetActive(true);
        m_carController.gameObject.transform.position = m_startPosition;
        m_carController.gameObject.transform.eulerAngles = m_startEularAngle;

        // TODO:
        // Code to stop car from moving
        // Error: When the user switches from the finish race and start race quickly
        //        the car will continue to roll


        m_followCamera.SetFollowTarget(m_carController.transform);
        m_UIManager.StartCountDown();
    }

    public void StartRace()
    {
        m_carController.isUserControlOn = true;
        m_recordingManager.StartRecording();
        m_recordingManager.RestartReplays();
    }

    public void FinishedRace(float _finishTime)
    {
        m_recordingManager.FinishRecording();

        m_carController.isUserControlOn = false;
        m_carController.Brakes();
        m_followCamera.SetFollowTarget(m_finishCameraSpot);

        GameObject replayObject = m_recordingManager.GetReplayObject(m_recordingManager.m_replayCount - 1);
        m_replayCustomiserCanvas.GetComponent<ReplayCustomisationManager>().InitializeReplayCustomizer(replayObject, _finishTime);
        GoToReplayCustomiser();
    }

    public void RemoveLastReplay()
    {
        m_recordingManager.DeleteReplay(m_recordingManager.m_replayCount - 1);
    }

    public void OnCheckPointTrigger(bool _finishPoint)
    {
        m_UIManager.CheckPointHit(_finishPoint);
    }



    private void SetCanvasesState(bool _state)
    {
        m_mainMenuCanvas.enabled = _state;
        m_UICanvas.enabled = _state;
        m_replayCustomiserCanvas.enabled = _state;
        m_leaderboardCanvas.enabled = _state;
    }
}