using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDemo_ReplayObject : ReplayObject
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

    protected override void ApplyInitialFrame()
    {
        frontLeftMesh.transform.localEulerAngles  = ReadVector3();
        frontRightMesh.transform.localEulerAngles = ReadVector3();
        rearLeftMesh.transform.localEulerAngles   = ReadVector3();
        rearRightMesh.transform.localEulerAngles  = ReadVector3();

        if (ReadBool()) RLWParticleSystem.Play();
        if (ReadBool()) RRWParticleSystem.Play();

        RLWTireSkid.emitting = ReadBool();
        RRWTireSkid.emitting = ReadBool();

        transform.position    = ReadVector3();
        transform.eulerAngles = ReadVector3();
    }

    protected override void UpdateReplay()
    {
        frontLeftMesh.transform.localEulerAngles  += RetrieveVector3();
        frontRightMesh.transform.localEulerAngles += RetrieveVector3();
        rearLeftMesh.transform.localEulerAngles   += RetrieveVector3();
        rearRightMesh.transform.localEulerAngles  += RetrieveVector3();

        if (ReadBool())
            RLWParticleSystem.Play();
        else
            RLWParticleSystem.Stop();

        if (ReadBool())
            RRWParticleSystem.Play();
        else
            RRWParticleSystem.Stop();

        RLWTireSkid.emitting = ReadBool();
        RRWTireSkid.emitting = ReadBool();

        transform.position    += RetrieveVector3();
        transform.eulerAngles += RetrieveVector3();
    }
}
