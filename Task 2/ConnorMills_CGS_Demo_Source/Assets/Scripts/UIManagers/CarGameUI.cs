using TMPro;
using UnityEngine;

public class CarGameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_timerText;
    [SerializeField] private TextMeshProUGUI m_checkPointsCounterText;
    [SerializeField] private TextMeshProUGUI m_lapCounterText;
    [SerializeField] private TextMeshProUGUI m_countDownText;
    [SerializeField] private TextMeshProUGUI m_pauseText;

    [SerializeField, Range(1, 10)] private int m_numberOfLaps = 1;

    private RecordingManager m_recordingManager;
    private ThirdPersonCar m_followCamera;
    private PrometeoCarController m_carController;
    private CheckPoints[] m_checkPoints;

    private int m_lapsCompleted;
    private int m_checkPointPassed;
    private int m_maxCheckPoints;

    private float timer;

    private bool countDown = false;
    private float countDownTimer = 2.99f;

    private bool isRacing = false;
    private bool isPaused = false;

    private void Start()
    {
        GameObject[] checkPoints = GameObject.FindGameObjectsWithTag("CheckPoint");
        m_maxCheckPoints = checkPoints.Length;

        m_checkPoints = new CheckPoints[m_maxCheckPoints];
        for(int i = 0; i < m_maxCheckPoints; i++)
        {
            m_checkPoints[i] = checkPoints[i].GetComponent<CheckPoints>();
            m_checkPoints[i].OnHit().AddListener(CheckPointHit);
        }
        GameObject.FindGameObjectWithTag("FinishPoint").GetComponent<CheckPoints>().OnHit().AddListener(CheckPointHit);

        ResetUI();
    }

    private void Update()
    {
        if (isRacing)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!isPaused)
                {
                    isPaused = true;
                    m_carController.isUserControlOn = false;
                    m_pauseText.enabled = true;
                    m_recordingManager.PauseReplays();
                    m_recordingManager.PauseRecording();
                    m_followCamera.Paused(true);
                    Time.timeScale = 0;
                }
                else
                {
                    isPaused = false;
                    m_carController.isUserControlOn = true;
                    m_pauseText.enabled = false;
                    m_recordingManager.PlayReplays();
                    m_recordingManager.ContinueRecording();
                    m_followCamera.Paused(false);
                    Time.timeScale = 1;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if(countDown)
        {
            if(countDownTimer <= -0.75f)
            {
                countDown = false;
                m_countDownText.enabled = false;
            }
            else if(countDownTimer <= 0)
            {
                if(!isRacing)
                {
                    CarGameManager.Instance.StartRace();
                    m_carController.isUserControlOn = true;
                    SetUIElementState(true);
                    m_countDownText.text = "GO";
                    isRacing = true;
                }
            }
            else
                m_countDownText.text = (Mathf.FloorToInt(countDownTimer + 1)).ToString();

            countDownTimer -= Time.deltaTime;
        }

        if(isRacing && !isPaused)
        {
            timer += Time.deltaTime;
            m_timerText.text = CarGameManager.Instance.GetTimeDisplay(timer);
        }
    }

    public void StartCountDown(RecordingManager _recordingManager, ThirdPersonCar _followCamera, PrometeoCarController _carController)
    {
        ResetUI();
        m_recordingManager = _recordingManager;
        if(m_recordingManager.HasReplays())
        {
            m_recordingManager.RestartReplays();
            m_recordingManager.PauseReplays();
        }
        m_followCamera = _followCamera;
        m_carController = _carController;
        m_carController.isUserControlOn = false;

        m_countDownText.text = (Mathf.FloorToInt(countDownTimer + 1)).ToString();
        m_countDownText.enabled = true;
        countDown = true;
    }

    public void CheckPointHit(bool _finish)
    {
        if(!_finish)
        {
            m_checkPointPassed++;
            m_checkPointsCounterText.text = m_checkPointPassed.ToString() + "/" + m_maxCheckPoints.ToString();
            return;
        }

        if (m_checkPointPassed == m_maxCheckPoints)
        {
            m_lapsCompleted++;
            m_checkPointPassed = 0;

            if (m_lapsCompleted == m_numberOfLaps)
            {
                isRacing = false;
                SetUIElementState(false);

                CarGameManager.Instance.FinishedRace(timer);
            }

            m_checkPointsCounterText.text = "0/" + m_maxCheckPoints.ToString();
            m_lapCounterText.text = "Lap " + (m_lapsCompleted + 1).ToString() + " of " + m_numberOfLaps.ToString();

            foreach (CheckPoints checkPoint in m_checkPoints)
                checkPoint.ResetCheckPoint();
        }
    }

    public void ResetUI()
    {
        m_lapsCompleted = 0;
        m_checkPointPassed = 0;

        timer = 0;
        countDownTimer = 2.99f;
        countDown = false;

        isRacing = false;

        foreach (CheckPoints checkPoint in m_checkPoints)
            checkPoint.ResetCheckPoint();

        m_pauseText.enabled = false;
        SetUIElementState(false);
    }

    private void SetUIElementState(bool _state)
    {
        m_timerText.text = "00:00.00";
        m_checkPointsCounterText.text = "0/" + m_maxCheckPoints.ToString();
        m_lapCounterText.text = "Lap 1 of " + m_numberOfLaps.ToString();
        m_countDownText.text = "3";

        m_timerText.enabled = _state;
        m_checkPointsCounterText.enabled = _state;
        m_lapCounterText.enabled = _state;
        m_countDownText.enabled = _state;
    }
}