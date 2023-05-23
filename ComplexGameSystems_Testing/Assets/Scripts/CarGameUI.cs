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
        m_checkPointsCounterText.text = "0/" + m_maxCheckPoints.ToString();
        m_lapCounterText.text = "Lap 1 of " + m_numberOfLaps.ToString();

        m_timerText.enabled = false;
        m_checkPointsCounterText.enabled = false;
        m_lapCounterText.enabled = false;
        m_countDownText.enabled = false;

        m_finishScreen.SetActive(false);
    }

    private void FixedUpdate()
    {
        if(countDown)
        {
            if(countDownTimer <= -0.5f)
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
                    m_timerText.enabled = true;
                    m_checkPointsCounterText.enabled = true;
                    m_lapCounterText.enabled = true;
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

            m_timerText.text = "";
            int mineuts = (int)timer / 60;
            if (mineuts < 10)
                m_timerText.text += "0" + mineuts.ToString();
            else
                m_timerText.text += mineuts.ToString();

            m_timerText.text += ":";

            int seconds = (int)timer % 60;
            if (seconds < 10)
                m_timerText.text += "0" + seconds.ToString();
            else
                m_timerText.text += seconds.ToString();
        }
    }

    public void StartCountDown()
    {
        countDownTimer = 2.99f;
        m_countDownText.text = ((int)countDownTimer).ToString();
        m_countDownText.enabled = true;
        countDown = true;

        m_finishScreen.SetActive(false);
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
                    m_timerText.enabled = false;
                    m_checkPointsCounterText.enabled = false;
                    m_lapCounterText.enabled = false;

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
}
