using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class CarGameManager : Singleton<CarGameManager>
{
    [SerializeField] private ThirdPersonCar m_followCamera;
    [SerializeField] private PrometeoCarController m_carController;
    [SerializeField] private Transform m_finishCameraTarget;

    [Space (20)]
    [SerializeField] private Canvas m_mainMenuCanvas;
    [SerializeField] private Canvas m_UICanvas;
    [SerializeField] private Canvas m_RaceViewCanvas;
    [SerializeField] private Canvas m_replayCustomiserCanvas;
    [SerializeField] private Canvas m_leaderboardCanvas;

    [Space (20)]
    [SerializeField] private RecordingManager m_recordingManager;

    private CarGameUI m_UIManager;
    private RaceViewManager m_raceViewManager;

    private Vector3 m_startPosition;
    private Vector3 m_startEularAngle;

    private void Start()
    {
        m_carController.isUserControlOn = false;
        m_startPosition = m_carController.gameObject.transform.position;
        m_startEularAngle = m_carController.gameObject.transform.eulerAngles;
        m_followCamera.SetFollowTarget(m_finishCameraTarget);

        GoToMainMenu();

        m_UIManager = m_UICanvas.GetComponent<CarGameUI>();
        m_raceViewManager = m_RaceViewCanvas.GetComponent<RaceViewManager>();
    }

    public void WatchAllReplay()
    {
        GoToRaceView();
        m_carController.gameObject.SetActive(false);
        m_raceViewManager.UpdateReplayObjects(m_recordingManager);
        m_raceViewManager.StartCountDown();
    }

    public void WatchLastReplay()
    {
        GoToInGameUI();
        m_UIManager.StartCountDown(m_recordingManager.m_replayCount - 1);
    }

    public void Quit()
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
    
    public void GoToRaceView()
    {
        SetCanvasesState(false);
        m_RaceViewCanvas.enabled = true;
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
        GoToInGameUI();
        m_UIManager.StartCountDown(m_startPosition, m_startEularAngle);
    }

    public void CustomizeReplay(GameObject _replayObject, float _finishTime)
    {
        m_replayCustomiserCanvas.GetComponent<ReplayCustomisationManager>().InitializeReplayCustomizer(_replayObject, _finishTime);
        GoToReplayCustomiser();
    }

    public void RemoveLastReplay()
    {
        m_recordingManager.DeleteReplay(m_recordingManager.m_replayCount - 1);
    }

    public string GetTimeDisplay(float _time)
    {
        string output = "";

        // Minutes
        int minutes = (int)_time / 60;
        if (minutes < 10)
            output += "0" + minutes.ToString();
        else
            output += minutes.ToString();

        // Seconds
        int seconds = (int)_time % 60;
        if (seconds < 10)
            output += ":0" + seconds.ToString();
        else
            output += ":" + seconds.ToString();

        // Milliseconds
        int milliseconds = (int)((_time - (int)_time) * 1000);
        if (milliseconds < 100)
            output += ".00" + milliseconds.ToString();
        else if(milliseconds < 10)
            output += ".0" + milliseconds.ToString();
        else
            output += "." + milliseconds.ToString();

        return output;
    }

    private void SetCanvasesState(bool _state)
    {
        m_mainMenuCanvas.enabled = _state;
        m_UICanvas.enabled = _state;
        m_RaceViewCanvas.enabled = _state;
        m_replayCustomiserCanvas.enabled = _state;
        m_leaderboardCanvas.enabled = _state;
    }



    public ThirdPersonCar FollowCamera() { return m_followCamera; }
    public PrometeoCarController CarController() { return m_carController; }
    public RecordingManager RecordingManager() { return m_recordingManager; }
    public Transform FinishCameraTarget() { return m_finishCameraTarget; }
}