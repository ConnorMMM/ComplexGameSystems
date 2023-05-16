using System.IO;
using UnityEngine;

public abstract class ReplayObject : MonoBehaviour
{
    private MemoryStream m_memoryStream = null;
    protected BinaryReader m_binaryReader = null;
    protected MemoryStreamSettings m_settings;

    private bool replaying = false;

    private void OnDestroy()
    {
        ClearMemory();
    }

    private void FixedUpdate()
    {
        if (replaying)
        {
            if (m_memoryStream.Position >= m_memoryStream.Length)
            {
                PauseReplay();
                return;
            }
            UpdateReplay();
        }
    }

    protected abstract void UpdateReplay();

    protected abstract void ApplyInitialFrame();


    public void InitializeReplayObject(MemoryStream _memoryStream, MemoryStreamSettings _settings, string _name)
    {
        gameObject.name = _name;
        m_settings = _settings;
        m_memoryStream = _memoryStream;
        m_binaryReader = new BinaryReader(_memoryStream);
        ResetReplay();
        SetVisibility(false);
        replaying = false;
    }

    public bool RestartReplay()
    {
        if (m_memoryStream == null)
        {
            Debug.Log($"MemoryStream is not set for {gameObject}");
            return false;
        }

        ResetReplay();
        SetVisibility(true);
        replaying = true;

        return true;
    }

    public void PlayReplay()
    {
        if(m_memoryStream == null)
        {
            Debug.Log($"MemoryStream is not set for {gameObject}");
            return;
        }

        SetVisibility(true);
        replaying = true;
    }

    public void PauseReplay()
    {
        replaying = false;
    }

    public void StopReplay()
    {
        SetVisibility(false);
        replaying = false;
        ResetReplay();
    }


    private void ClearMemory()
    {
        m_memoryStream.Dispose();
        m_binaryReader.Dispose();
    }

    private void SetVisibility(bool _state)
    {
        gameObject.SetActive(_state);
    }

    private void ResetReplay()
    {
        m_memoryStream.Seek(0, SeekOrigin.Begin);
        ApplyInitialFrame();
    }


    protected Vector3 ReadVector3()
    {
        Vector3 vec3;
        vec3.x = m_binaryReader.ReadSingle();
        vec3.y = m_binaryReader.ReadSingle();
        vec3.z = m_binaryReader.ReadSingle();
        return vec3;
    }

    protected bool ReadBool()
    {
        return m_binaryReader.ReadBoolean();
    }

    protected void ApplyReplayPosition()
    {
        if (m_binaryReader.ReadBoolean())
            transform.position += ReadVector3();
    }
    protected void ApplyReplayPosition(Transform _transform)
    {
        if (m_binaryReader.ReadBoolean())
            _transform.position += ReadVector3();
    }

    protected void ApplyReplayRotation()
    {
        if (m_binaryReader.ReadBoolean())
            transform.eulerAngles += ReadVector3();
    }
    protected void ApplyReplayRotation(Transform _transform)
    {
        if (m_binaryReader.ReadBoolean())
            _transform.eulerAngles += ReadVector3();
    }
    protected void ApplyReplayLocalRotation(Transform _transform)
    {
        if (m_binaryReader.ReadBoolean())
            _transform.localEulerAngles += ReadVector3();
    }

    protected void ApplyReplayScale()
    {
        if (m_binaryReader.ReadBoolean())
            transform.localScale += ReadVector3();
    }
    protected void ApplyReplayScale(Transform _transform)
    {
        if (m_binaryReader.ReadBoolean())
            _transform.localScale += ReadVector3();
    }
}
