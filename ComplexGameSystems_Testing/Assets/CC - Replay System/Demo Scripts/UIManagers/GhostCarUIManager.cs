using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GhostCarUIManager : MonoBehaviour
{
    [SerializeField] private Canvas m_carUICanvas;
    [SerializeField] private TextMeshProUGUI m_InitialsText;

    public float time { get; private set; }
    public string initials { get; private set; }

    public void InitialiseCarUI(float _time, string _initials)
    {
        time = _time;
        initials = _initials;
        m_InitialsText.text = _initials;
    }
}
