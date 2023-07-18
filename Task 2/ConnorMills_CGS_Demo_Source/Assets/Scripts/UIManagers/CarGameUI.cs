using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarGameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_timerText;
    [SerializeField] private TextMeshProUGUI m_checkPointsCounterText;
    [SerializeField] private TextMeshProUGUI m_lapCounterText;
    [SerializeField] private TextMeshProUGUI m_countDownText;

    [Space(20)]
    [SerializeField] private GameObject m_pauseMenu;
    [SerializeField] private Button m_continueButton;
    [SerializeField] private Button m_exitButton;

    [Space(20)]
    [SerializeField, Range(1, 10)] private int m_numberOfLaps = 1;

    private RecordingManager m_recordingManager;
    private ThirdPersonCar m_followCamera;
    private PrometeoCarController m_carController;
    private CheckPoints[] m_checkPoints;


    private int m_lapsCompleted;
    private int m_checkPointPassed;
    private int m_maxCheckPoints;

    private float timer;

    private bool isRacing = false;

    private bool isReplay = false;
    private int replayIndex;

    private void Start()
    {
        GameObject[] checkPoints = GameObject.FindGameObjectsWithTag("CheckPoint");
        m_maxCheckPoints = checkPoints.Length;

        m_checkPoints = new CheckPoints[m_maxCheckPoints + 1];
        m_checkPoints[0] = GameObject.FindGameObjectWithTag("FinishPoint").GetComponent<CheckPoints>();
        for (int i = 0; i < m_maxCheckPoints; i++)
            m_checkPoints[i + 1] = checkPoints[i].GetComponent<CheckPoints>();

        m_recordingManager = CarGameManager.Instance.RecordingManager();
        m_followCamera = CarGameManager.Instance.FollowCamera();
        m_carController = CarGameManager.Instance.CarController();

        m_continueButton.onClick.AddListener(OnContinueClick);
        m_exitButton.onClick.AddListener(OnExitClick);
    }

    private void Update()
    {
        if (isRacing)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                TogglePause();
        }
    }

    private void FixedUpdate()
    {
        if(isRacing)
        {
            timer += Time.deltaTime;
            m_timerText.text = CarGameManager.Instance.GetTimeDisplay(timer);
        }
    }

    public void StartCountDown(Vector3 _startPos, Vector3 _startAngle)
    {
        if (m_recordingManager.HasReplays())
        {
            m_recordingManager.RestartReplays();
            m_recordingManager.PauseReplays();
        }

        m_carController.isUserControlOn = false;
        m_carController.gameObject.SetActive(true);
        m_carController.gameObject.transform.position = _startPos;
        m_carController.gameObject.transform.eulerAngles = _startAngle;

        m_followCamera.SetFollowTarget(m_carController.transform);

        StartCoroutine(CountDown());
    }

    public void StartCountDown(int _replayIndex)
    {
        isReplay = true;
        replayIndex = _replayIndex;

        m_recordingManager.StopReplays();
        m_recordingManager.RestartReplay(replayIndex);
        m_recordingManager.PauseReplay(replayIndex);

        GameObject replayObject = m_recordingManager.GetReplayObject(replayIndex);
        GameObject collider = Instantiate(m_carController.gameObject.GetComponentInChildren<MeshCollider>().gameObject, replayObject.transform);
        collider.layer = 8;
        collider.AddComponent<Rigidbody>().isKinematic = true;

        m_carController.isUserControlOn = false;
        m_carController.gameObject.SetActive(false);
        m_followCamera.SetFollowTarget(replayObject.transform);

        StartCoroutine(CountDown());
    }

    IEnumerator CountDown()
    {
        foreach (CheckPoints checkPoint in m_checkPoints)
            checkPoint.OnHit().AddListener(CheckPointHit);

        ResetRace();
        m_countDownText.text = "3";
        m_countDownText.enabled = true;
        yield return new WaitForSeconds(1f);

        m_countDownText.text = "2";
        yield return new WaitForSeconds(1f);
        
        m_countDownText.text = "1";
        yield return new WaitForSeconds(1f);

        m_countDownText.text = "GO";
        StartRace();
        yield return new WaitForSeconds(.75f);

        m_countDownText.enabled = false;
    }

    public void CheckPointHit(bool _finish)
    {
        if(_finish)
        {
            if (m_checkPointPassed == m_maxCheckPoints)
            {
                m_lapsCompleted++;
                if (m_lapsCompleted == m_numberOfLaps)
                {
                    isRacing = false;
                    FinishRace();
                    return;
                }

                m_checkPointPassed = 0;
                m_lapCounterText.text = "Lap " + (m_lapsCompleted + 1).ToString() + " of " + m_numberOfLaps.ToString();

                foreach (CheckPoints checkPoint in m_checkPoints)
                    checkPoint.ResetCheckPoint();
            }
        }
        else
        {
            m_checkPointPassed++;
        }

        m_checkPointsCounterText.text = m_checkPointPassed.ToString() + "/" + m_maxCheckPoints.ToString();
    }

    private void ResetRace()
    {
        m_lapsCompleted = 0;
        m_lapCounterText.text = "Lap 1 of " + m_numberOfLaps.ToString();

        m_checkPointPassed = 0;
        m_checkPointsCounterText.text = "0/" + m_maxCheckPoints.ToString();

        timer = 0;
        m_timerText.text = "00:00.00";

        isRacing = false;

        foreach (CheckPoints checkPoint in m_checkPoints)
            checkPoint.ResetCheckPoint();

        m_pauseMenu.SetActive(false);
        SetUIElements(false);
    }

    private void SetUIElements(bool _state)
    {
        m_lapCounterText.enabled = _state;
        m_checkPointsCounterText.enabled = _state;
        m_timerText.enabled = _state;
        m_countDownText.enabled = _state;
    }

    private void StartRace()
    {
        if (!isReplay)
        {
            m_carController.isUserControlOn = true;
            m_recordingManager.StartRecording();
            m_recordingManager.PlayReplays();
        }
        else
        {
            m_recordingManager.PlayReplay(replayIndex);
        }

        SetUIElements(true);
        isRacing = true;
    }

    private void FinishRace()
    {
        m_followCamera.SetFollowTarget(CarGameManager.Instance.FinishCameraTarget());
        foreach (CheckPoints checkPoint in m_checkPoints)
            checkPoint.OnHit().RemoveAllListeners();

        if (!isReplay)
        {
            m_recordingManager.StopRecording();
            m_carController.isUserControlOn = false;
            GameObject replayObject = m_recordingManager.GetReplayObject(m_recordingManager.m_replayCount - 1);

            CarGameManager.Instance.CustomizeReplay(replayObject, timer);
        }
        else
        {
            isReplay = false;
            GameObject replayObject = m_recordingManager.GetReplayObject(replayIndex);
            Destroy(replayObject.GetComponentInChildren<MeshCollider>().gameObject);

            CarGameManager.Instance.CustomizeReplay(replayObject, timer);
        }
    }

    private void TogglePause()
    {
        m_pauseMenu.SetActive(!m_pauseMenu.activeSelf);
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
    }

    private void OnContinueClick()
    {
        TogglePause();
    }

    private void OnExitClick()
    {
        m_recordingManager.PauseReplays();
        if(!isReplay)
        {
            m_recordingManager.ClearRecording();
            m_carController.isUserControlOn = false;
        }

        foreach (CheckPoints checkPoint in m_checkPoints)
            checkPoint.OnHit().RemoveAllListeners();
        isRacing = false;
        Time.timeScale = 1;

        m_followCamera.SetFollowTarget(CarGameManager.Instance.FinishCameraTarget());

        CarGameManager.Instance.GoToMainMenu();
    }
}