using UnityEngine;

public class Transform_Recorder : Recorder
{
    private Vector3 m_PositionPrevFrame;
    private Vector3 m_RotationPrevFrame;
    private Vector3 m_ScalePrevFrame;

    protected override void CollectPreviousFrame()
    {
        m_PositionPrevFrame = transform.position;
        m_RotationPrevFrame = transform.eulerAngles;
        m_ScalePrevFrame    = transform.lossyScale;
    }

    protected override void RecordInitialFrame()
    {
        SaveVector3(transform.position);
        SaveVector3(transform.eulerAngles);
        SaveVector3(transform.lossyScale);
    }

    protected override void UpdateRecording()
    {
        CheckVector3Diff(transform.position,    m_PositionPrevFrame);
        CheckVector3Diff(transform.eulerAngles, m_RotationPrevFrame);
        CheckVector3Diff(transform.lossyScale,  m_ScalePrevFrame);
    }
}
