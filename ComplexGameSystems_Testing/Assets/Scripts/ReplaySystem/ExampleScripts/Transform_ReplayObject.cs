using UnityEngine;

public class Transform_ReplayObject : ReplayObject
{
    protected override void ApplyInitialFrame()
    {
        if (m_settings.UsePosition()) transform.position    = ReadVector3();
        if (m_settings.UseRotation()) transform.eulerAngles = ReadVector3();
        if (m_settings.UseScale())    transform.localScale  = ReadVector3();
    }

    protected override void UpdateReplay()
    {
        if (m_settings.UsePosition()) ApplyReplayPosition();
        if (m_settings.UseRotation()) ApplyReplayRotation();
        if (m_settings.UseScale())    ApplyReplayScale();
    }
}
