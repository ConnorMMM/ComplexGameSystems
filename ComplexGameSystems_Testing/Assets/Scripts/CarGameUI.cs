using TMPro;
using UnityEngine;

public class CarGameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_timerText;
    [SerializeField] private TextMeshProUGUI m_checkPointsCounterText;
    [SerializeField] private TextMeshProUGUI m_lapCounterText;
    [SerializeField] private TextMeshProUGUI m_countDownText;
    [SerializeField] private TextMeshProUGUI m_finishTimeText;

    [SerializeField] private GameObject m_finishScreen;

    [SerializeField, Range(1, 10)] private int m_numberOfLaps = 1;

    private int m_lapsCompleted = 1;
    private int m_checkPointPassed = 0;
    private int m_maxCheckPoints = 0;

    private float timer = 0;

    private bool countDown = false;
    private float countDownTimer = 2.99f;

    private bool isRacing = false;

    void Start()
    {
        GameObject[] checkPoints = GameObject.FindGameObjectsWithTag("CheckPoint");
        m_maxCheckPoints = checkPoints.Length;

        ResetRace();
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

            if(countDownTimer <= 0)
            {
                if(!isRacing)
                {
                    CarGameManager.Instance.StartRace();
                    m_countDownText.text = "GO";
                    SetInRaceUI(true);
                    isRacing = true;
                }
            }
            else
                m_countDownText.text = (Mathf.FloorToInt(countDownTimer + 1)).ToString();

            countDownTimer -= Time.deltaTime;
        }

        if(isRacing)
        {
            timer += Time.deltaTime;
            m_timerText.text = GetTimeDisplay(timer);
        }
    }

    public void StartCountDown()
    {
        ResetRace();

        m_countDownText.text = (Mathf.FloorToInt(countDownTimer + 1)).ToString();
        m_countDownText.enabled = true;
        countDown = true;
    }

    public void CheckPointHit(bool _finish)
    {
        if (_finish)
        {
            if (m_checkPointPassed == m_maxCheckPoints)
            {
                if(m_lapsCompleted == m_numberOfLaps)
                {
                    isRacing = false;
                    SetInRaceUI(false);

                    m_finishScreen.SetActive(true);
                    m_finishTimeText.text = m_timerText.text;

                    CarGameManager.Instance.FinishedLinePassed(timer);
                }
                else
                {
                    m_lapsCompleted++;
                    m_checkPointPassed = 0;
                    m_checkPointsCounterText.text = "0/" + m_maxCheckPoints.ToString();
                    m_lapCounterText.text = "Lap " + m_lapsCompleted.ToString() + " of " + m_numberOfLaps.ToString();
                    return;
                }
            }
            return;
        }

        m_checkPointPassed++;
        m_checkPointsCounterText.text = m_checkPointPassed.ToString() + "/" + m_maxCheckPoints.ToString();
    }

    public float GetFinishTime()
    {
        return timer;
    }

    private void SetInRaceUI(bool _state)
    {
        if(_state && !m_timerText.enabled)
        {
            m_checkPointsCounterText.text = "0/" + m_maxCheckPoints.ToString();
            m_lapCounterText.text = "Lap 1 of " + m_numberOfLaps.ToString();
        }
        m_timerText.enabled = _state;
        m_checkPointsCounterText.enabled = _state;
        m_lapCounterText.enabled = _state;
    }

    private string GetTimeDisplay(float _time)
    {
        string output = "";

        int minutes = (int)_time / 60;
        if (minutes < 10)
            output += "0" + minutes.ToString();
        else
            output += minutes.ToString();

        output += ":";

        int seconds = (int)_time % 60;
        if (seconds < 10)
            output += "0" + seconds.ToString();
        else
            output += seconds.ToString();

        return output;
    }

    private void ResetRace()
    {
        isRacing = false;
        countDown = false;

        timer = 0;
        countDownTimer = 2.99f;

        m_lapsCompleted = 1;
        m_checkPointPassed = 0;

        m_countDownText.enabled = false;
        SetInRaceUI(false);
        m_finishScreen.SetActive(false);
    }
}
