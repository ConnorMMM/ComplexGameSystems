using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField] private Button m_watchAllReplaysButton;
    [SerializeField] private Button m_watchLastReplayButton;
    [SerializeField] private Button m_retryButton;
    [SerializeField] private Button m_mainMenuButton;

    private void Awake()
    {
        m_watchAllReplaysButton.onClick.AddListener(OnWatchAllReplaysClick);
        m_watchLastReplayButton.onClick.AddListener(OnWatchLastReplayClick);
        m_retryButton.onClick.AddListener(OnRetryClick);
        m_mainMenuButton.onClick.AddListener(OnMainMenuClick);
    }

    private void OnWatchAllReplaysClick()
    {
        CarGameManager.Instance.WatchAllReplay();
    }

    private void OnWatchLastReplayClick()
    {
        CarGameManager.Instance.WatchLastReplay();
    }

    private void OnRetryClick()
    {
        CarGameManager.Instance.InitialiseRace();
    }

    private void OnMainMenuClick()
    {
        CarGameManager.Instance.GoToMainMenu();
    }
}
