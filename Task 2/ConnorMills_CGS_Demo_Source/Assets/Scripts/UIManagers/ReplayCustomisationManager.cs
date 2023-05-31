using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReplayCustomisationManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_timeText;

    [SerializeField] private Scrollbar m_redScrollbar;
    [SerializeField] private Scrollbar m_greenScrollbar;
    [SerializeField] private Scrollbar m_blueScrollbar;
    [SerializeField] private Image m_colourBlock;

    [SerializeField] private Button m_saveButton;
    [SerializeField] private Button m_removeButton;

    private Vector3 m_replayColor;
    private MeshRenderer[] m_meshRenderers;

    private GameObject m_replayObject;
    private float m_time;
    private char[] m_initials;

    private void Awake()
    {
        m_replayColor = Vector3.one;
        UpdateScrollbars();

        m_redScrollbar.onValueChanged.AddListener(OnRedScrollbarChange);
        m_greenScrollbar.onValueChanged.AddListener(OnGreenScrollbarChange);
        m_blueScrollbar.onValueChanged.AddListener(OnBlueScrollbarChange);

        m_saveButton.onClick.AddListener(OnSaveButtonClick);
        m_removeButton.onClick.AddListener(OnRemoveButtonClick);

        m_initials = new char[3];
        for(int i = 0; i < 3; i++)
            m_initials[i] = 'A';
    }

    private void OnRedScrollbarChange(float _value)
    {
        m_replayColor.x = _value;
        UpdateColours();
    }

    private void OnGreenScrollbarChange(float _value)
    {
        m_replayColor.y = _value;
        UpdateColours();
    }

    private void OnBlueScrollbarChange(float _value)
    {
        m_replayColor.z = _value;
        UpdateColours();
    }

    private void OnSaveButtonClick()
    {
        string initials = "";
        foreach(char initial in m_initials)
        {
            initials += initial.ToString();
        }
        m_replayObject.GetComponent<GhostCarUIManager>().InitialiseCarUI(m_time, initials);

        CarGameManager carGameManager = CarGameManager.Instance;

        // TODO:
        // Save Time and Initials to the leaderboard


        carGameManager.GoToLeaderboard();
    }

    private void OnRemoveButtonClick()
    {
        CarGameManager carGameManager = CarGameManager.Instance;
        carGameManager.RemoveLastReplay();
        carGameManager.GoToLeaderboard();
    }

    public void InitializeReplayCustomizer(GameObject _replayObject, float _time)
    {
        m_time = _time;
        UpdateTimeText(_time);

        m_replayObject = _replayObject;
        m_meshRenderers = m_replayObject.GetComponentsInChildren<MeshRenderer>();
        GetColorFromObject();
    }

    public void UpdateInitials(char _initial, int _index)
    {
        m_initials[_index - 1] = _initial;
    }

    private void UpdateColours()
    {
        m_colourBlock.color = new Color(m_replayColor.x, m_replayColor.y, m_replayColor.z, 1);

        if(m_meshRenderers.Length != 0)
        {
            m_meshRenderers[0].material.color = new Color(m_replayColor.x, m_replayColor.y, m_replayColor.z, 0.47f);
        }
    }

    private void GetColorFromObject()
    {
        if (m_meshRenderers.Length != 0)
        {
            m_replayColor.x = m_meshRenderers[0].material.color.r;
            m_replayColor.y = m_meshRenderers[0].material.color.g;
            m_replayColor.z = m_meshRenderers[0].material.color.b;
        }

        m_colourBlock.color = new Color(m_replayColor.x, m_replayColor.y, m_replayColor.z, 1);
        UpdateScrollbars();
    }

    private void UpdateTimeText(float _time)
    {
        string output = "Your Time ";

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
        if (milliseconds < 10)
            output += ".00" + milliseconds.ToString();
        else if(milliseconds < 100)
            output += ".0" + milliseconds.ToString();
        else
            output += "." + milliseconds.ToString();

        m_timeText.text = output;
    }

    private void UpdateScrollbars()
    {
        m_redScrollbar.value = m_replayColor.x;
        m_greenScrollbar.value = m_replayColor.y;
        m_blueScrollbar.value = m_replayColor.z;
    }
}
