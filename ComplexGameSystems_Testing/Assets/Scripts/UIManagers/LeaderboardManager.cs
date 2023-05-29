using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField] private Button m_watchReplayButton;
    [SerializeField] private Button m_retryButton;
    [SerializeField] private Button m_mainMenuButton;

    private void Awake()
    {
        m_watchReplayButton.onClick.AddListener(OnWatchReplayClick);
        m_retryButton.onClick.AddListener(OnRetryClick);
        m_mainMenuButton.onClick.AddListener(OnMainMenuClick);
    }

    private void OnWatchReplayClick()
    {

    }

    private void OnRetryClick()
    {

    }

    private void OnMainMenuClick()
    {
        CarGameManager.Instance.GoToMainMenu();
    }
}
