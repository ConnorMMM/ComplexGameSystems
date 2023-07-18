using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RaceViewManager : MonoBehaviour
{
    [SerializeField] private Transform m_birdsEyeView;
    [SerializeField] private TextMeshProUGUI m_timerText;
    [SerializeField] private TextMeshProUGUI m_countDownText;

    [SerializeField] private GameObject m_pauseMenu;
    [SerializeField] private Button m_continueButton;
    [SerializeField] private Button m_ExitButton;

    private RecordingManager m_recordingManager;
    private ReplayObject[] m_replayObjects;
    private ThirdPersonCar m_followCamera;

    private Transform[] m_cameraViews;
    private int m_currentViewIndex;

    private float timer;
    private bool isRacing = false;
    private bool isInScene = false;

    private void Start()
    {
        m_followCamera = CarGameManager.Instance.FollowCamera();

        m_cameraViews = new Transform[1];
        m_cameraViews[0] = m_birdsEyeView;

        m_continueButton.onClick.AddListener(OnContinueClick);
        m_ExitButton.onClick.AddListener(OnExitClick);

        ResetRace();
    }

    private void Update()
    {
        if (isInScene)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (m_currentViewIndex > 0)
                    m_currentViewIndex--;
                else
                    m_currentViewIndex = m_cameraViews.Length - 1;

                m_followCamera.SetFollowTarget(m_cameraViews[m_currentViewIndex]);
            }
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (m_currentViewIndex < m_cameraViews.Length - 1)
                    m_currentViewIndex++;
                else
                    m_currentViewIndex = 0;

                m_followCamera.SetFollowTarget(m_cameraViews[m_currentViewIndex]);
            }

            if (isRacing)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                    TogglePaused();
            }
        }
    }

    private void FixedUpdate()
    {
        if (isRacing)
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
        ResetRace();

        m_followCamera.SetFollowTarget(m_cameraViews[m_currentViewIndex]);
        isInScene = true;

        m_recordingManager.RestartReplays();
        m_recordingManager.PauseReplays();

        StartCoroutine(CountDown());
    }

    IEnumerator CountDown()
    {
        m_countDownText.text = "3";
        m_countDownText.enabled = true;
        yield return new WaitForSeconds(1f);

        m_countDownText.text = "2";
        yield return new WaitForSeconds(1f);

        m_countDownText.text = "1";
        yield return new WaitForSeconds(1f);

        m_countDownText.text = "RACE";
        m_timerText.enabled = true;
        isRacing = true;
        m_recordingManager.PlayReplays();
        yield return new WaitForSeconds(.75f);

        m_countDownText.enabled = false;
    }

    public void ResetRace()
    {
        m_currentViewIndex = 0;

        timer = 0;
        isRacing = false;
        isInScene = false;

        m_timerText.text = "00:00.00";

        m_timerText.enabled = false;
        m_countDownText.enabled = false;
        m_pauseMenu.SetActive(false);
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

    private void TogglePaused()
    {
        m_pauseMenu.SetActive(!m_pauseMenu.activeSelf);
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
    }

    private void OnContinueClick()
    {
        TogglePaused();
    }

    private void OnExitClick()
    {
        m_recordingManager.PauseReplays();
        isRacing = false;
        isInScene = false;
        Time.timeScale = 1;

        m_followCamera.SetFollowTarget(CarGameManager.Instance.FinishCameraTarget());

        CarGameManager.Instance.GoToMainMenu();
    }
}
