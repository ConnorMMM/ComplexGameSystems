using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button m_StartButton;
    [SerializeField] private Button m_QuitButton;

    private void Awake()
    {
        m_StartButton.onClick.AddListener(OnStartClick);
        m_QuitButton.onClick.AddListener(OnQuitClick);
    }

    private void OnStartClick()
    {
        CarGameManager.Instance.InitialiseRace();
    }

    private void OnQuitClick()
    {
        CarGameManager.Instance.Quit();
    }
}
