using UnityEngine;

public class CarDemo_Recorder : Recorder
{
    [Header("WHEELS")]
    public GameObject frontLeftMesh;
    public GameObject frontRightMesh;
    public GameObject rearLeftMesh;
    public GameObject rearRightMesh;

    //PARTICLE SYSTEMS
    [Space(20)]
    [Header("EFFECTS")]
    // The following particle systems are used as tire smoke when the car drifts.
    public ParticleSystem RLWParticleSystem;
    public ParticleSystem RRWParticleSystem;
    [Space(10)]
    // The following trail renderers are used as tire skids when the car loses traction.
    public TrailRenderer RLWTireSkid;
    public TrailRenderer RRWTireSkid;

    // Private variables

    private Vector3 frontLeftPrevRotation;
    private Vector3 frontRightPrevRotation;
    private Vector3 rearLeftPrevRotation;
    private Vector3 rearRightPrevRotation;

    private Vector3 m_PositionPrevFrame;
    private Vector3 m_RotationPrevFrame;

    protected override void CollectPreviousFrame()
    {
        frontLeftPrevRotation  = frontLeftMesh.transform.localEulerAngles;
        frontRightPrevRotation = frontRightMesh.transform.localEulerAngles;
        rearLeftPrevRotation   = rearLeftMesh.transform.localEulerAngles;
        rearRightPrevRotation  = rearRightMesh.transform.localEulerAngles;

        m_PositionPrevFrame    = transform.position;
        m_RotationPrevFrame    = transform.eulerAngles;
    }

    protected override void RecordInitialFrame()
    {
        SaveVector3(frontLeftMesh.transform.localEulerAngles);
        SaveVector3(frontRightMesh.transform.localEulerAngles);
        SaveVector3(rearLeftMesh.transform.localEulerAngles);
        SaveVector3(rearRightMesh.transform.localEulerAngles);

        SaveBool(RLWParticleSystem.isEmitting);
        SaveBool(RRWParticleSystem.isEmitting);

        SaveBool(RLWTireSkid.emitting);
        SaveBool(RRWTireSkid.emitting);

        SaveVector3(transform.position);
        SaveVector3(transform.eulerAngles);
    }

    protected override void UpdateRecording()
    {
        CheckVector3Diff(frontLeftMesh.transform.localEulerAngles,  frontLeftPrevRotation);
        CheckVector3Diff(frontRightMesh.transform.localEulerAngles, frontRightPrevRotation);
        CheckVector3Diff(rearLeftMesh.transform.localEulerAngles,   rearLeftPrevRotation);
        CheckVector3Diff(rearRightMesh.transform.localEulerAngles,  rearRightPrevRotation);

        SaveBool(RLWParticleSystem.isEmitting);
        SaveBool(RRWParticleSystem.isEmitting);

        SaveBool(RLWTireSkid.emitting);
        SaveBool(RRWTireSkid.emitting);

        CheckVector3Diff(transform.position,    m_PositionPrevFrame);
        CheckVector3Diff(transform.eulerAngles, m_RotationPrevFrame);
    }
}
