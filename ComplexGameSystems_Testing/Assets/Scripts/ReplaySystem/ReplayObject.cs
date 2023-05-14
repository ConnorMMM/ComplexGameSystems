using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ReplayObject : MonoBehaviour
{
    private MemoryStream memoryStream = null;
    private BinaryReader binaryReader = null;

    private bool replaying;

    private void FixedUpdate()
    {
        if (replaying)
            UpdateReplay();
    }

    private void UpdateReplay()
    {
        if (memoryStream.Position >= memoryStream.Length)
        {
            StopReplaying();
            return;
        }

        ReadTransform();
    }

    public void StartReplay()
    {
        if(memoryStream == null)
        {
            Debug.Log($"MemoryStream is not set for {gameObject}");
            return;
        }

        ResetReplayFrame();
        replaying = true;
    }

    public void StopReplaying()
    {
        replaying = false;
        Debug.Log("Stopped Replaying");
    }

    public void PlayReplay()
    {
        replaying = true;
    }

    public void PauseReplay()
    {
        replaying = false;
    }

    private void ResetReplayFrame()
    {
        memoryStream.Seek(0, SeekOrigin.Begin);
    }

    private void ReadTransform()
    {
        if(binaryReader.ReadBoolean())
        {
            float x = binaryReader.ReadSingle();
            float y = binaryReader.ReadSingle();
            float z = binaryReader.ReadSingle();
            Debug.Log($"x: {x}, y: {y}, z: {z}");
            gameObject.transform.position = new Vector3(x, y, z);
        }
        else
        {
            Debug.Log("Empty");
        }
    }

    public void SetMemoryStream(MemoryStream _memoryStream)
    {
        memoryStream = _memoryStream;
        binaryReader = new BinaryReader(memoryStream);
        ResetReplayFrame();
    }
}
