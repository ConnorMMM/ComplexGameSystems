using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RaceViewManager : MonoBehaviour
{
    [SerializeField] private Transform m_birdsEyeView;
    [SerializeField] private TextMeshProUGUI m_timerText;
    [SerializeField] private TextMeshProUGUI m_countDownText;
    [SerializeField] private TextMeshProUGUI m_PauseText;

    private RecordingManager m_recordingManager;
    private ReplayObject[] m_replayObjects;
    private ThirdPersonCar m_thirdPersonCar;

    private Transform[] m_cameraViews;
    private int m_currentViewIndex;

    private float timer;
    private bool isRacing = false;
    private bool isPaused = false;
    private bool isInScene = false;

    private float countDownTimer = 2.99f;
    private bool countDown = false;

    void Awake()
    {
        m_thirdPersonCar = Camera.main.GetComponent<ThirdPersonCar>();

        m_cameraViews = new Transform[1];
        m_cameraViews[0] = m_birdsEyeView;

        ResetUI();
    }

    private void FixedUpdate()
    {
        if (isInScene)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (m_currentViewIndex > 0)
                    m_currentViewIndex--;
                else
                    m_currentViewIndex = m_cameraViews.Length - 1;

                m_thirdPersonCar.SetFollowTarget(m_cameraViews[m_currentViewIndex]);
            }
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (m_currentViewIndex < m_cameraViews.Length - 1)
                    m_currentViewIndex++;
                else
                    m_currentViewIndex = 0;

                m_thirdPersonCar.SetFollowTarget(m_cameraViews[m_currentViewIndex]);
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!isPaused)
                {
                    isPaused = true;
                    m_PauseText.enabled = true;
                    m_recordingManager.PauseReplays();
                    m_thirdPersonCar.Paused(true);
                }
                else
                {
                    isPaused = false;
                    m_PauseText.enabled = false;
                    m_recordingManager.PlayReplays();
                    m_thirdPersonCar.Paused(false);
                }
            }
        }

        if (countDown)
        {
            if (countDownTimer <= -0.75f)
            {
                countDown = false;
                m_countDownText.enabled = false;
            }
            else if (countDownTimer <= 0)
            {
                if (!isRacing)
                {
                    m_timerText.enabled = true;
                    m_countDownText.text = "RACE";
                    isRacing = true;
                    m_recordingManager.PlayReplays();
                }
            }
            else
                m_countDownText.text = (Mathf.FloorToInt(countDownTimer + 1)).ToString();

            countDownTimer -= Time.deltaTime;
        }

        if (isRacing && !isPaused)
        {
            timer += Time.deltaTime;
            m_timerText.text = CarGameManager.Instance.GetTimeDisplay(timer);

            bool allFinished = true;
            foreach(ReplayObject replayObject in m_replayObjects)
            {
                if(replayObject.IsReplaying())
                {
                    allFinished = false;
                    break;
                }
            }
            if(allFinished)
            {
                isRacing = false;
                isInScene = false;
                CarGameManager.Instance.GoToLeaderboard();
            }
        }
    }

    public void StartCountDown()
    {
        ResetUI();

        m_thirdPersonCar.SetFollowTarget(m_cameraViews[m_currentViewIndex]);

        m_countDownText.text = (Mathf.FloorToInt(countDownTimer + 1)).ToString();
        m_countDownText.enabled = true;
        countDown = true;
        isInScene = true;

        m_recordingManager.RestartReplays();
        m_recordingManager.PauseReplays();
    }

    public void ResetUI()
    {
        m_currentViewIndex = 0;

        timer = 0;
        isRacing = false;
        isPaused = false;
        isInScene = false;
        countDownTimer = 2.99f;
        countDown = false;

        m_timerText.text = "00:00.00";
        m_countDownText.text = "3";

        m_timerText.enabled = false;
        m_countDownText.enabled = false;
        m_PauseText.enabled = false;
    }

    public void UpdateReplayObjects(RecordingManager _recordingManager)
    {
        m_recordingManager = _recordingManager;
        GameObject[] replayObjects = m_recordingManager.GetReplayObjects();

        m_replayObjects = new ReplayObject[replayObjects.Length];
        m_cameraViews = new Transform[replayObjects.Length + 1];

        m_cameraViews[0] = m_birdsEyeView;
        for (int i = 0; i < m_replayObjects.Length; i++)
        {
            m_replayObjects[i] = replayObjects[i].GetComponent<ReplayObject>();
            m_cameraViews[i + 1] = replayObjects[i].transform;
        }
    }
}
