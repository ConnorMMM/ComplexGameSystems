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

    private void Start()
    {
        m_carController.enabled = false;

        m_mainMenu.enabled = true;
        m_UICanvas.enabled = false;
    }

    public void OnStartPress()
    {
        m_mainMenu.enabled = false;
        m_UICanvas.enabled = true;

        m_followCamera.SetFollowTarget(m_carController.gameObject.transform);
        m_inGameUI.StartCountDown();
    }

    public void OnRestartPress()
    {
        m_followCamera.SetFollowTarget(m_carController.gameObject.transform);
        m_inGameUI.StartCountDown();
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
        Debug.Log("Finished :" + _finishTime.ToString());
        // TODO: Put code for finishing
        // Menu swaping
        m_followCamera.SetFollowTarget(null);
    }

    public void StartRace()
    {
        m_carController.enabled = true;
        m_recordingManager.StartRecording();
    }
}