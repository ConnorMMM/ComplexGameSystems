using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarGameManager : MonoBehaviour
{
    [SerializeField] private Canvas m_mainMenu;
    [SerializeField] private CarGameUI m_inGameUI;

    [SerializeField] private PrometeoCarController m_carController;

    [SerializeField] private RecordingManager m_recordingManager;

    private int m_checkPointPassed = 0;
    private int m_maxCheckPoints = 0;

    void Start()
    {
        m_carController.enabled = false;

        GameObject[] checkPoints = GameObject.FindGameObjectsWithTag("CheckPoint");
        m_maxCheckPoints = checkPoints.Length;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStartPress()
    {
        m_carController.enabled = true;
        m_mainMenu.enabled = false;
        m_recordingManager.StartRecording();
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
        if(_finishPoint)
        {
            if(m_checkPointPassed == m_maxCheckPoints)
            {
                m_recordingManager.FinishRecording();
                m_recordingManager.PlayReplay();

                // TODO: Put code for finishing
                // Menu swaping
            }
            return;
        }

        m_checkPointPassed++;

        Debug.Log("CheckPoint");
    }
}
