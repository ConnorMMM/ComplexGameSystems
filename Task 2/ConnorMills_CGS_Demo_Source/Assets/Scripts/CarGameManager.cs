using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class CarGameManager : Singleton<CarGameManager>
{
    [SerializeField] private ThirdPersonCar m_followCamera;
    [SerializeField] private PrometeoCarController m_carController;
    [SerializeField] private Transform m_finishCameraSpot;

    [Space (20)]
    [SerializeField] private Canvas m_mainMenuCanvas;
    [SerializeField] private Canvas m_UICanvas;
    [SerializeField] private Canvas m_RaceViewCanvas;
    [SerializeField] private Canvas m_replayCustomiserCanvas;
    [SerializeField] private Canvas m_leaderboardCanvas;

    [Space(20)]
    [SerializeField] private RecordingManager m_recordingManager;

    private CarGameUI m_UIManager;
    private RaceViewManager m_raceViewManager;

    private Vector3 m_startPosition;
    private Vector3 m_startEularAngle;

    private bool m_isWatchingReplay;
    private int m_watchReplayIndex;

    private void Start()
    {
        m_isWatchingReplay = false;
        m_watchReplayIndex = 0;

        m_carController.isUserControlOn = false;
        m_startPosition = m_carController.gameObject.transform.position;
        m_startEularAngle = m_carController.gameObject.transform.eulerAngles;

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
        m_isWatchingReplay = true;
        m_watchReplayIndex = m_recordingManager.m_replayCount - 1;
        InitialiseRace();
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

        if(!m_isWatchingReplay)
        {
            m_recordingManager.RestartReplays();
            m_recordingManager.PauseReplays();

            m_carController.gameObject.SetActive(true);
            m_carController.gameObject.transform.position = m_startPosition;
            m_carController.gameObject.transform.eulerAngles = m_startEularAngle;

            // TODO:
            // Code to stop car from moving
            // Error: When the user switches from the finish race and start race quickly
            //        the car will continue to roll


            m_followCamera.SetFollowTarget(m_carController.transform);
        }
        else
        {
            m_recordingManager.StopReplays();
            m_recordingManager.RestartReplay(m_watchReplayIndex);
            m_recordingManager.PauseReplay(m_watchReplayIndex);

            GameObject replayObject = m_recordingManager.GetReplayObject(m_watchReplayIndex);
            GameObject collider = Instantiate(m_carController.gameObject.GetComponentInChildren<MeshCollider>().gameObject, replayObject.transform);
            collider.layer = 8;
            collider.AddComponent<Rigidbody>().isKinematic = true;

            m_carController.gameObject.SetActive(false);

            m_followCamera.SetFollowTarget(m_recordingManager.GetReplayTransform(m_watchReplayIndex));
        }

        m_UIManager.StartCountDown();
    }

    public void StartRace()
    {
        if(!m_isWatchingReplay)
        {
            m_carController.isUserControlOn = true;
            m_recordingManager.StartRecording();
            m_recordingManager.PlayReplays();
        }
        else
        {
            m_recordingManager.PlayReplay(m_watchReplayIndex);
        }
    }

    public void FinishedRace(float _finishTime)
    {
        if (!m_isWatchingReplay)
        {
            m_recordingManager.FinishRecording();

            m_carController.isUserControlOn = false;
            m_carController.Brakes();

            GameObject replayObject = m_recordingManager.GetReplayObject(m_recordingManager.m_replayCount - 1);
            m_replayCustomiserCanvas.GetComponent<ReplayCustomisationManager>().InitializeReplayCustomizer(replayObject, _finishTime);
            GoToReplayCustomiser();
        }
        else
        {
            m_isWatchingReplay = false;
            GameObject replayObject = m_recordingManager.GetReplayObject(m_watchReplayIndex);
            Destroy(replayObject.GetComponentInChildren<MeshCollider>().gameObject);

            m_replayCustomiserCanvas.GetComponent<ReplayCustomisationManager>().InitializeReplayCustomizer(replayObject, _finishTime);
            GoToReplayCustomiser();
        }
        
        m_followCamera.SetFollowTarget(m_finishCameraSpot);
    }

    public void RemoveLastReplay()
    {
        m_recordingManager.DeleteReplay(m_recordingManager.m_replayCount - 1);
    }

    public void OnCheckPointTrigger(bool _finishPoint)
    {
        m_UIManager.CheckPointHit(_finishPoint);
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

    private void AddCarBodyCollider(GameObject _object)
    {
        Instantiate(m_carController.gameObject.GetComponentInChildren<MeshCollider>().gameObject, _object.transform);
    }
}