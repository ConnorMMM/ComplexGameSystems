using UnityEngine;

public class Transform_ReplayObject : ReplayObject
{
    protected override void ApplyInitialFrame()
    {
        transform.position    = ReadVector3();
        transform.eulerAngles = ReadVector3();
        transform.localScale  = ReadVector3();
    }

    protected override void UpdateReplay()
    {
        transform.position    += RetrieveVector3();
        transform.eulerAngles += RetrieveVector3();
        transform.localScale  += RetrieveVector3();
    }
}
