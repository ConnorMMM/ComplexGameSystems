using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/MemoryStreamSettingsScriptableObject", order = 1)]
public class MemoryStreamSettings : ScriptableObject
{
    [SerializeField] private bool m_usePosition;
    [SerializeField] private bool m_useRotation;
    [SerializeField] private bool m_useScale;

    public bool UsePosition() { return m_usePosition; }
    public bool UseRotation() { return m_useRotation; }
    public bool UseScale() { return m_useScale; }
}
