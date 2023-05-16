using UnityEngine;

public class Transform_Recorder : Recorder
{
    private Vector3 m_PositionPrevFrame;
    private Vector3 m_RotationPrevFrame;
    private Vector3 m_ScalePrevFrame;

    protected override void CollectPreviousFrame()
    {
        if (m_settings.UsePosition()) m_PositionPrevFrame = transform.position;
        if (m_settings.UseRotation()) m_RotationPrevFrame = transform.eulerAngles;
        if (m_settings.UseScale())    m_ScalePrevFrame = transform.lossyScale;
    }

    protected override void RecordInitialFrame()
    {
        if (m_settings.UsePosition()) SaveVector3(transform.position);
        if (m_settings.UseRotation()) SaveVector3(transform.eulerAngles);
        if (m_settings.UseScale())    SaveVector3(transform.lossyScale);
    }

    protected override void UpdateRecording()
    {
        if (m_settings.UsePosition()) CheckVector3Diff(transform.position, m_PositionPrevFrame);
        if (m_settings.UseRotation()) CheckVector3Diff(transform.eulerAngles, m_RotationPrevFrame);
        if (m_settings.UseScale())    CheckVector3Diff(transform.lossyScale, m_ScalePrevFrame);
    }
}
