using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InitialManager : MonoBehaviour
{
    [SerializeField] private ReplayCustomisationManager m_replayCustomizationManager;
    [SerializeField, Range(1, 3)] private int m_initialIndex;

    [SerializeField] private TextMeshProUGUI m_initialText;
    [SerializeField] private Button m_upButton;
    [SerializeField] private Button m_downButton;

    private int m_charIndex = 0;

    private void Awake()
    {
        m_upButton.onClick.AddListener(OnUpButtonClick);
        m_downButton.onClick.AddListener(OnDownButtonClick);
    }

    private void OnUpButtonClick()
    {
        if (m_charIndex < 25)
            m_charIndex++;
        else
            m_charIndex = 0;
        
        UpdateInitial();
    }

    private void OnDownButtonClick()
    {
        if (m_charIndex > 0)
            m_charIndex--;
        else
            m_charIndex = 25;

        UpdateInitial();
    }

    private void UpdateInitial()
    {
        m_initialText.text = ((char)(m_charIndex + 65)).ToString();

        if (m_replayCustomizationManager != null || m_initialIndex != 0)
            m_replayCustomizationManager.UpdateInitials((char)(m_charIndex + 65), m_initialIndex);
    }
}
