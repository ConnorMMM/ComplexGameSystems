using UnityEngine;

public class Transform_TagRecorder : TagRecorder
{
    private Vector3[] m_PositionsPrevFrame;
    private Vector3[] m_RotationsPrevFrame;
    private Vector3[] m_ScalesPrevFrame;

    protected override void CollectPreviousFrameAt(int _index)
    {
        m_PositionsPrevFrame[_index] = m_gameObjects[_index].transform.position;
        m_RotationsPrevFrame[_index] = m_gameObjects[_index].transform.localEulerAngles;
        m_ScalesPrevFrame[_index]    = m_gameObjects[_index].transform.lossyScale;
    }

    protected override void RecordInitialFrameAt(int _index)
    {
        SaveVector3(m_gameObjects[_index].transform.position);
        SaveVector3(m_gameObjects[_index].transform.localEulerAngles);
        SaveVector3(m_gameObjects[_index].transform.lossyScale);
    }

    protected override void UpdateRecordingAt(int _index)
    {
        CheckVector3Diff(m_gameObjects[_index].transform.position,         m_PositionsPrevFrame[_index]);
        CheckVector3Diff(m_gameObjects[_index].transform.localEulerAngles, m_RotationsPrevFrame[_index]);
        CheckVector3Diff(m_gameObjects[_index].transform.lossyScale,       m_ScalesPrevFrame[_index]);
    }

    protected override void SetRecorderLists(int _length)
    {
        m_PositionsPrevFrame = new Vector3[_length];
        m_RotationsPrevFrame = new Vector3[_length];
        m_ScalesPrevFrame    = new Vector3[_length];
    }
}
